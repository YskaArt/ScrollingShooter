using UnityEngine;

public class EnemyBasic : EnemyBase
{
    public float lateralSpeed = 2f;
    public float lateralFrequency = 2f; // Velocidad de oscilación lateral
    private float lateralOffset;

    protected override void Awake()
    {
        base.Awake();
        lateralOffset = Random.Range(0f, 15f * Mathf.PI); // Variación por enemigo
    }

    protected override void Move()
    {
        if (Camera.main == null) return;

        Vector3 forwardDir = Vector3.back * moveSpeed;
        float lateralMovement = Mathf.Sin(Time.time * lateralFrequency + lateralOffset) * 15f * lateralSpeed;

        Vector3 newPos = rb.position + forwardDir * Time.fixedDeltaTime;
        newPos.x += lateralMovement * Time.fixedDeltaTime;

        
        Vector3 clampedPos = ClampToViewport(newPos);
        rb.MovePosition(clampedPos);
    }

    private Vector3 ClampToViewport(Vector3 worldPos)
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(worldPos);
        viewportPos.x = Mathf.Clamp(viewportPos.x, 0.05f, 0.95f); // Margen
        Vector3 clampedWorld = Camera.main.ViewportToWorldPoint(viewportPos);
        return new Vector3(clampedWorld.x, rb.position.y, clampedWorld.z);
    }
}
