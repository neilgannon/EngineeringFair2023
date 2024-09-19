using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyBase EnemyPrefab;
    [SerializeField] private float spawnTime = 2;
    [SerializeField] private int MaxSawnCount = 5;
    private int currentSpawCount = 0;

    void Start()
    {
        InvokeRepeating("SpawnEnemy", spawnTime, spawnTime);
    }

    void SpawnEnemy()
    {
        if (currentSpawCount < MaxSawnCount)
        {
            Vector2 position = transform.position;
            position += Random.insideUnitCircle * 3;

            Instantiate(EnemyPrefab, position, Quaternion.identity);
            currentSpawCount++;
        }
        else
        {
            CancelInvoke("SpawnEnemy");
        }
    }
}
