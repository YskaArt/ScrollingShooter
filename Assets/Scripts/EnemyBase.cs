using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Rigidbody))]
public abstract class EnemyBase : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int maxHealth = 3;
    public VisualEffect explosionEffectPrefab;
    public int Points;
    protected int currentHealth;
    protected Rigidbody rb;

    [Header("Drops")]
    public List<DropItem> dropTable;
    public AudioClip deathSound;        
    protected AudioSource audioSource;


    [System.Serializable]
    public class DropItem
    {
        public GameObject itemPrefab;
        [Range(0f, 1f)]
        public float dropChance = 0.25f;
    }

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
    }

    protected virtual void FixedUpdate()
    {
        Move();
    }

    protected abstract void Move();

    public virtual void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        GameObject soundObject = new GameObject("DeathSound");
        AudioSource tempAudio = soundObject.AddComponent<AudioSource>();
        tempAudio.clip = deathSound;
        tempAudio.Play();
        Destroy(soundObject, deathSound.length);

        if (explosionEffectPrefab != null)
        { 
            VisualEffect vfx = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            Destroy(vfx.gameObject, 3f);
        }
        UIManager.Instance.AddScore(Points);
        TryDropItem();
        Destroy(gameObject);
    }

    private void TryDropItem()
    {
        foreach (var drop in dropTable)
        {
            float roll = Random.value;
            if (roll <= drop.dropChance)
            {
                GameObject spawnedItem = Instantiate(drop.itemPrefab, transform.position, Quaternion.identity);

                if (spawnedItem.TryGetComponent<Rigidbody>(out var rb))
                {
                    rb.linearVelocity = Vector3.down * 2f; // Movimiento hacia abajo
                }

                // Se destruye si el player no lo toma
                Destroy(spawnedItem, 30f);

                break; // Solo un drop permitido
            }
        }
    }

  
}
