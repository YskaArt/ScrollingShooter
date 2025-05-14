using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float tiltAngle = 25f;
    public float tiltSmoothTime = 0.1f;

    [Header("Disparo")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;
    public float fireRate = 0.25f;

    [Header("Cámara")]
    public Camera mainCamera;
    public bool isInSpecialCameraView = false;

    [Header("Límites Viewport")]
    [Range(0f, 1f)] public float minX = 0.05f;
    [Range(0f, 1f)] public float maxX = 0.95f;
    [Range(0f, 1f)] public float minY = 0.05f;
    [Range(0f, 1f)] public float maxY = 0.95f;

    private Vector2 moveInput;
    private Rigidbody rb;

    private Vector3 currentVelocity;
    private float currentZRotationVelocity;

    private bool isShooting;
    private float fireTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Movimiento: adaptado según el modo especial o no
        Vector3 inputDirection = isInSpecialCameraView
            ? new Vector3(moveInput.x, moveInput.y, 0f)   // Vertical en modo especial
            : new Vector3(moveInput.x, 0f, moveInput.y);  // Normal

        Vector3 targetVelocity = inputDirection * moveSpeed;
        Vector3 smoothedVelocity = Vector3.SmoothDamp(rb.linearVelocity, targetVelocity, ref currentVelocity, 0.1f);
        rb.linearVelocity = smoothedVelocity;

        // Límite en pantalla
        Vector3 clampedPos = ClampPositionToViewport(rb.position);
        rb.position = clampedPos;

        // Rotación en eje Z si no es modo especial
        if (!isInSpecialCameraView)
        {
            float targetZRotation = -moveInput.x * tiltAngle;
            float currentZRotation = Mathf.SmoothDampAngle(transform.localEulerAngles.z, targetZRotation, ref currentZRotationVelocity, tiltSmoothTime);
            Vector3 currentRotation = transform.localEulerAngles;
            transform.localEulerAngles = new Vector3(currentRotation.x, currentRotation.y, currentZRotation);
        }
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
        }
        else if (context.canceled)
        {
            isShooting = false;
        }
    }

    private void Shoot()
    {
        
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            if (bullet.TryGetComponent<Rigidbody>(out var bulletRb))
            {
                bulletRb.linearVelocity = firePoint.forward * bulletSpeed;
            }
        
    }
    public void SetSpecialCameraMode(bool active)
    {
        isInSpecialCameraView = active;

        if (active)
        {
            // Forzar posición Z del player a -11 (manteniendo X e Y actuales)
            Vector3 pos = rb.position;
            pos.z = -11f;
            rb.position = pos;
            // Congelar rotación en Z
            rb.constraints |= RigidbodyConstraints.FreezeRotationZ;

           
        }
        else
        {
            // Liberar rotación en Z
            rb.constraints &= ~RigidbodyConstraints.FreezeRotationZ;
        }
    }
}
