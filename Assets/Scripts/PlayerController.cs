using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float tiltAngle = 25f;
    public float tiltSmoothTime = 0.1f;

    [Header("Disparo")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float fireRate = 0.25f;
    public Transform[] firePoints; // 0 = centro, 1-2 = laterales, 3-4 = extremos
    [Range(1, 4)] public int shotLevel = 1;

    [Header("Cámara")]
    public Camera mainCamera;

    [Header("Límites Viewport")]
    [Range(0f, 1f)] public float minX = 0.05f;
    [Range(0f, 1f)] public float maxX = 0.95f;
    [Range(0f, 1f)] public float minY = 0.05f;
    [Range(0f, 1f)] public float maxY = 0.95f;

    [Header("Salud")]
    public int maxHealth = 5;
    private int currentHealth;

    private Vector2 moveInput;
    private Rigidbody rb;
    private Vector3 currentVelocity;
    private float currentZRotationVelocity;

    private bool isShooting;
    private float fireTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
    }

    private void FixedUpdate()
    {
        Vector3 inputDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        Vector3 targetVelocity = inputDirection * moveSpeed;
        Vector3 smoothedVelocity = Vector3.SmoothDamp(rb.linearVelocity, targetVelocity, ref currentVelocity, 0.1f);
        rb.linearVelocity = smoothedVelocity;

        Vector3 clampedPos = ClampPositionToViewport(rb.position);
        rb.position = clampedPos;

        float targetZRotation = -moveInput.x * tiltAngle;
        float currentZRotation = Mathf.SmoothDampAngle(transform.localEulerAngles.z, targetZRotation, ref currentZRotationVelocity, tiltSmoothTime);
        Vector3 currentRotation = transform.localEulerAngles;
        transform.localEulerAngles = new Vector3(currentRotation.x, currentRotation.y, currentZRotation);
    }

    private void Update()
    {
        if (isShooting)
        {
            fireTimer += Time.deltaTime;
            if (fireTimer >= fireRate)
            {
                Shoot();
                fireTimer = 0f;
            }
        }
    }

    private Vector3 ClampPositionToViewport(Vector3 worldPos)
    {
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(worldPos);
        viewportPos.x = Mathf.Clamp(viewportPos.x, minX, maxX);
        viewportPos.y = Mathf.Clamp(viewportPos.y, minY, maxY);
        Vector3 clampedWorld = mainCamera.ViewportToWorldPoint(viewportPos);
        return new Vector3(clampedWorld.x, rb.position.y, clampedWorld.z);
    }

    // Input Actions
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isShooting = true;
            fireTimer = fireRate;
            Shoot();
            fireTimer = 0f;
        }
        else if (context.canceled)
        {
            isShooting = false;
        }
    }

    private void Shoot()
    {
        // Lógica de disparo basada en el nivel
        switch (shotLevel)
        {
            case 1:
                FireFromPoint(0); // central
                break;
            case 2:
                FireFromPoint(1);
                FireFromPoint(2);
                break;
            case 3:
                FireFromPoint(0);
                FireFromPoint(1);
                FireFromPoint(2);
                break;
            case 4:
                FireFromPoint(0);
                FireFromPoint(1);
                FireFromPoint(2);
                FireFromPoint(3);
                FireFromPoint(4);
                break;
        }
    }

    private void FireFromPoint(int index)
    {
        if (index >= 0 && index < firePoints.Length)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoints[index].position, firePoints[index].rotation);
            if (bullet.TryGetComponent<Rigidbody>(out var bulletRb))
            {
                bulletRb.linearVelocity = firePoints[index].forward * bulletSpeed;
            }
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public void UpgradeShot()
    {
        if (shotLevel < 4)
            shotLevel++;
    }

    private void Die()
    {
        Debug.Log("El jugador ha muerto.");
        // Aquí puedes pausar el juego o mostrar el Game Over
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUpHeal"))
        {
            Heal(1);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("PowerUpShot"))
        {
            UpgradeShot();
            Destroy(other.gameObject);
        }
    }
}
