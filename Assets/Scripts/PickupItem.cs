using UnityEngine;

public enum PowerUpType
{
    Heal,
    FireUpgrade
}

public class PowerUp : MonoBehaviour
{
    public PowerUpType type;
    public int healAmount = 1;
    public float fallSpeed = 2f;
    public float lifeTime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.Translate(Vector3.back * fallSpeed * Time.deltaTime);
    }

   
}
