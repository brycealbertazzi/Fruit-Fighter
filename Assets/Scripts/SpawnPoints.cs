using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    public float spawnRate;
    public float initialRoundSpawnDelay;
    [SerializeField] private Transform enemyParentTransform;
    [SerializeField] private GameObject enemy;

    void Start()
    {
        GameManager.instance.enemiesLeftInRound = 8;
        initialRoundSpawnDelay = Mathf.Clamp(initialRoundSpawnDelay, 1, 6);
        Invoke("SpawnEnemy", initialRoundSpawnDelay);
    }

    
    void SpawnEnemy() {
        Debug.Log("Enemy Spawned");
        if (GameManager.instance.enemiesLeftInRound > 0)
        {
            GameObject instEnemy = Instantiate(enemy, transform.position, Quaternion.identity, enemyParentTransform);
            GameManager.instance.enemiesLeftInRound--;
        }
    }

}
