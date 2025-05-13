using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;
    public float fireRate = 0.25f;
    public float tiltAngle = 25f;
    public float tiltSmoothTime = 0.1f;
    private float currentZRotationVelocity;


    private Vector2 moveInput;
    private Rigidbody rb;
    private Vector3 currentVelocity;

    private Coroutine shootCoroutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Movimiento 
        Vector3 targetVelocity = new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed;
        Vector3 smoothedVelocity = Vector3.SmoothDamp(rb.linearVelocity, targetVelocity, ref currentVelocity, 0.1f);
        rb.linearVelocity = smoothedVelocity;

        // Rotación 
        float targetZRotation = -moveInput.x * tiltAngle;
        float currentZRotation = Mathf.SmoothDampAngle(transform.localEulerAngles.z, targetZRotation, ref currentZRotationVelocity, tiltSmoothTime);

        // Aplicar rotación manteniendo los demás ejes
        Vector3 currentRotation = transform.localEulerAngles;
        transform.localEulerAngles = new Vector3(currentRotation.x, currentRotation.y, currentZRotation);
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            shootCoroutine = StartCoroutine(ContinuousShoot());
        }
        else if (context.canceled)
        {
            if (shootCoroutine != null)
                StopCoroutine(shootCoroutine);
        }
    }

    private IEnumerator ContinuousShoot()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(fireRate);
        }
    }

    private void Shoot()
    {
        if (bulletPrefab && firePoint)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            if (bullet.TryGetComponent<Rigidbody>(out var bulletRb))
            {
                bulletRb.linearVelocity = transform.forward * bulletSpeed;
            }
        }
    }
}
