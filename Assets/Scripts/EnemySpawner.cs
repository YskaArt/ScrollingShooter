using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemigos")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private int poolSizePerEnemy = 10; // cantidad de instancias por tipo
    [SerializeField] private float spawnInterval = 2f;

    [Header("Área de Spawn")]
    [SerializeField] private BoxCollider spawnArea;

    private float timer;
    private Dictionary<GameObject, Queue<GameObject>> enemyPools = new();

    void Start()
    {
        InitializePools();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void InitializePools()
    {
        foreach (GameObject prefab in enemyPrefabs)
        {
            Queue<GameObject> pool = new Queue<GameObject>();

            for (int i = 0; i < poolSizePerEnemy; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                pool.Enqueue(obj);
            }

            enemyPools.Add(prefab, pool);
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0 || spawnArea == null) return;

        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        GameObject enemy = GetPooledObject(prefab);

        if (enemy != null)
        {
            Vector3 spawnPos = GetRandomPointInBox(spawnArea);
            enemy.transform.position = spawnPos;
            enemy.transform.rotation = prefab.transform.rotation;
            enemy.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"No hay enemigos disponibles en el pool para {prefab.name}");
        }
    }

    GameObject GetPooledObject(GameObject prefab)
    {
        if (!enemyPools.ContainsKey(prefab)) return null;

        Queue<GameObject> pool = enemyPools[prefab];

        // Buscar uno inactivo
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
                return obj;
        }

       

        return null;
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
