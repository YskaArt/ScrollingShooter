using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    public float speed = 8f;
    public float homingDuration = 1.5f;
    private Transform target;
    private float homingTimer;
    public int damage = 2;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        homingTimer = homingDuration;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void FixedUpdate()
    {
        if (homingTimer > 0f && target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            rb.linearVelocity = direction * speed;

            // Rotaci�n para que el proyectil apunte hacia el jugador (solo en X y Z)
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.2f);

            homingTimer -= Time.fixedDeltaTime;
        }
        else
        {
            rb.linearVelocity = transform.forward * speed;
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
