using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemigos")]
    public GameObject[] enemyPrefabs;
    public float spawnInterval = 2f;

    [Header("Área de Spawn")]
    public BoxCollider spawnArea;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0 || spawnArea == null) return;

        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        Vector3 spawnPos = GetRandomPointInBox(spawnArea);
        Instantiate(prefab, spawnPos, prefab.transform.rotation);
    }

    Vector3 GetRandomPointInBox(BoxCollider box)
    {
        Vector3 center = box.center + box.transform.position;
        Vector3 size = box.size * 0.5f;

        float x = Random.Range(-size.x, size.x);
        float y = Random.Range(-size.y, size.y);
        float z = Random.Range(-size.z, size.z);

        return center + new Vector3(x, y, z);
    }

    private void OnDrawGizmos()
    {
        if (spawnArea == null) return;

        Gizmos.color = Color.red;
        Gizmos.matrix = spawnArea.transform.localToWorldMatrix;
        Gizmos.DrawWireCube(spawnArea.center, spawnArea.size);
    }
}
