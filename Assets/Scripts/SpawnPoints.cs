using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    public float timeBetweenSpawns;
    public float initialRoundSpawnDelay;
    [SerializeField] private Transform enemyParentTransform;
    [SerializeField] private GameObject enemy;

    void Start()
    {
        initialRoundSpawnDelay = Mathf.Clamp(initialRoundSpawnDelay, 1, 6);
        
    }

    public void SpawnEnemy()
    {
        if (GameManager.instance.state == GameManager.GameStates.GameOn && (GameManager.instance.totalEnemiesOnMap < GameManager.instance.maxEnemiesAllowedOnMap))
        {
            Debug.Log("Enemy Spawned");
            GameObject instEnemy = Instantiate(enemy, transform.position, Quaternion.identity, enemyParentTransform);
            GameManager.instance.totalEnemiesOnMap++;
        }
    }
}
