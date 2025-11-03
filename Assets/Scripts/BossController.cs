using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Rigidbody))]
public class BossController : EnemyBase
{
   
    public event Action<int, int> OnHealthChanged; // Vida actual / máxima
    public event Action OnBossDied;
    private List<ICommand> attackCommands;


    [Header("Ataque")]
    [SerializeField] private GameObject straightProjectilePrefab;
    [SerializeField] private GameObject homingProjectilePrefab;
    [SerializeField] private Transform[] straightFirePoints;
    [SerializeField] private Transform[] homingFirePoints;
    [SerializeField] private float fireRate = 2f;

    [Header("Movimiento")]
    [SerializeField] private float moveRange = 10f;
    private Vector3 startPos;
    private bool movingRight = true;

    [Header("Jugador")]
    [SerializeField] private Transform player;

    [Header("Muerte Boss")]
    [SerializeField] private float deathDelay = 2f;
    [SerializeField] private int deathVFXCount = 5;

    public BossState CurrentState { get; private set; }

    public int CurrentHealth => currentHealth;
    public float FireRate => fireRate;

   
    protected override void Awake()
    {
        base.Awake();
        startPos = transform.position;

        if (player == null)
            player = FindAnyObjectByType<PlayerController>()?.transform;
        attackCommands = new List<ICommand>
        {
            new FireStraightCommand(this),
            new FireHomingCommand(this)
        };
    }

    private void Start()
    {
        SwitchState(new BossAttackState(this));
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Update()
    {
        CurrentState?.UpdateState();
    }
    public void ExecuteAttackCommands()
    {
        foreach (var command in attackCommands)
        {
            command.Execute();
        }
    }

    public void SwitchState(BossState newState)
    {
        CurrentState?.ExitState();
        CurrentState = newState;
        CurrentState.EnterState();
    }

   
     public override void Move()
    {
        float direction = movingRight ? 1f : -1f;
        rb.linearVelocity = new Vector3(direction * moveSpeed, 0f, 0f);

        if (movingRight && transform.position.x >= startPos.x + moveRange)
            movingRight = false;
        else if (!movingRight && transform.position.x <= startPos.x - moveRange)
            movingRight = true;
    }

    public void FireStraightProjectiles()
    {
        foreach (Transform firePoint in straightFirePoints)
        {
            GameObject proj = Instantiate(straightProjectilePrefab, firePoint.position, firePoint.rotation);
            if (proj.TryGetComponent<Rigidbody>(out var rb))
                rb.linearVelocity = firePoint.forward * 10f;
        }
    }

    public void FireHomingProjectiles()
    {
        foreach (Transform firePoint in homingFirePoints)
        {
            GameObject proj = Instantiate(homingProjectilePrefab, firePoint.position, firePoint.rotation);
            if (proj.TryGetComponent<HomingProjectile>(out var homing))
                homing.SetTarget(player);
        }
    }

    // ===========================
    // VIDA Y MUERTE
    // ===========================
    public override void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            SwitchState(new BossDeathState(this));
            OnBossDied?.Invoke();
        }
    }

    public IEnumerator DeathSequence()
    {
        float interval = deathDelay / deathVFXCount;

        for (int i = 0; i < deathVFXCount; i++)
        {
            if (explosionEffectPrefab != null)
            {
                audioSource.PlayOneShot(deathSound);
                VisualEffect vfx = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
                Destroy(vfx.gameObject, 3f);
            }

            yield return new WaitForSeconds(interval);
        }

        GameManager.Instance.Victory();
        Die(); // ← Llama al método base (Drop + Score + Destroy)
    }
}
