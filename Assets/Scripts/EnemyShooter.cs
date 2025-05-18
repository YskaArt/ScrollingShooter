using UnityEngine;

public class EnemyShooter : EnemyBase
{
    [Header("Disparo")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;
    public float fireRate = 1f;

    private float fireTimer = 0f;
    private Transform player;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    protected override void Move()
    {
        if (player == null)
            return;

        // Verifica si el enemigo está dentro del viewport
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        bool isInViewport = viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1 && viewportPos.z > 0;

        if (isInViewport)
        {
            fireTimer += Time.deltaTime;

            if (fireTimer >= fireRate)
            {
                fireTimer = 0f;
                Shoot();
            }

            // Movimiento horizontal mientras dispara 
            Vector3 move = Vector3.right * Mathf.Sin(Time.time * 2f) * moveSpeed;
            rb.linearVelocity = new Vector3(move.x, 0f, 0f); 
        }
        else
        {
            // Se mueve hacia adelante hasta entrar en cámara
            rb.linearVelocity = Vector3.back * moveSpeed;
        }
    }

    private void Shoot()
    {
        if (bulletPrefab == null || firePoint == null || player == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        if (bullet.TryGetComponent<Rigidbody>(out var bulletRb))
        {
            Vector3 direction = (player.position - firePoint.position).normalized;
            bulletRb.linearVelocity = direction * bulletSpeed;
        }
        else
        {
            Debug.LogWarning("La bala no tiene Rigidbody asignado.");
        }
    }
}
