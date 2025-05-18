using UnityEngine;

public class Destroyer : MonoBehaviour
{

    public int damage = 99;
    private void OnTriggerEnter(Collider other)
    {
        EnemyBase enemy = other.GetComponent<EnemyBase>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            
        }
    }
}
