using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Rigidbody))]
public abstract class EnemyBase : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int maxHealth = 3;
    public VisualEffect explosionEffectPrefab;

    protected int currentHealth;
    protected Rigidbody rb;

    [Header("Drops")]
    public List<DropItem> dropTable;


    [System.Serializable]
    public class DropItem
    {
        public GameObject itemPrefab;
        [Range(0f, 1f)]
        public float dropChance = 0.25f;
    }

    protected virtual void Awake()
    {
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

    protected virtual void Die()
    {
        TryDropItem();

        if (explosionEffectPrefab != null)
        {
            VisualEffect vfx = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            Destroy(vfx.gameObject, 3f);
        }

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
                Destroy(spawnedItem, 10f);

                break; // Solo un drop permitido
            }
        }
    }

  
}
