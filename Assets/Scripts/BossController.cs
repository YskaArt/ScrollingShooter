using UnityEngine;
using System.Collections;
using UnityEngine.VFX;
using UnityEngine.Audio;

public class BossController : EnemyBase
{

    public int CurrentHealth => currentHealth;
    [Header("Disparo")]
    public GameObject straightProjectilePrefab;
    public GameObject homingProjectilePrefab;
    public Transform[] straightFirePoints; // 3 FirePoints
    public Transform[] homingFirePoints;   // 2 FirePoints
    public float fireRate = 2f;
    private float fireTimer;

    [Header("Movimiento")]
    public float moveRange = 10f;
    private Vector3 startPos;
    private bool movingRight = true;

    [Header("Jugador")]
    public Transform player;

    
    public float deathDelay = 2f;     
    public int deathVFXCount = 5;

    protected override void Awake()
    {

        base.Awake();
        startPos = transform.position;
        fireTimer = fireRate;
        if (player == null)
            player = FindAnyObjectByType<PlayerController>()?.transform;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f)
        {
            FireStraightProjectiles();
            FireHomingProjectiles();
            fireTimer = fireRate;
        }

      
    }

  

    protected override void Move()
    {
        float direction = movingRight ? 1f : -1f;
        rb.linearVelocity = new Vector3(direction * moveSpeed, 0f, 0f);

        if (movingRight && transform.position.x >= startPos.x + moveRange)
        {
            movingRight = false;
        }
        else if (!movingRight && transform.position.x <= startPos.x - moveRange)
        {
            movingRight = true;
        }
    }

    void FireStraightProjectiles()
    {
        foreach (Transform firePoint in straightFirePoints)
        {
            GameObject proj = Instantiate(straightProjectilePrefab, firePoint.position, firePoint.rotation);
            if (proj.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.linearVelocity = firePoint.forward * 10f;
            }
        }
    }

    void FireHomingProjectiles()
    {
        foreach (Transform firePoint in homingFirePoints)
        {
            GameObject proj = Instantiate(homingProjectilePrefab, firePoint.position, firePoint.rotation);
            if (proj.TryGetComponent<HomingProjectile>(out var homing))
            {
                homing.SetTarget(player);
            }
        }
    }

   
    public override void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;

            // Forzar visualmente la barra de vida a 0
            BossHealthUI bossUI = FindAnyObjectByType<BossHealthUI>();
            if (bossUI != null && bossUI.healthFill != null)
                bossUI.healthFill.fillAmount = 0;

            
            StartCoroutine(DeathSequence());
        }
    }

    private IEnumerator DeathSequence()
    {
        float duration = deathDelay;
        float interval = duration / deathVFXCount;

        for (int i = 0; i < deathVFXCount; i++)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)
            );

            if (explosionEffectPrefab != null)
            {
                audioSource.PlayOneShot(deathSound);
                VisualEffect vfx = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
                Destroy(vfx.gameObject, 3f);
            }

            yield return new WaitForSeconds(interval);
        }
       
     
        GameManager.Instance.Victory();


    }

}
