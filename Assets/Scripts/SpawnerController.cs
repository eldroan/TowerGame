using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SpawnerController : MonoBehaviour
{
    [SerializeField] private int maxEnemies;
    [SerializeField] private int currentEnemies;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnRate;
    private float _timeToNextSpawn;
    private Vector3 _spawnPosition;

    private void Awake()
    {
        //Assert.IsNotNull(enemyPrefab);
        _spawnPosition = this.transform.position;
        _timeToNextSpawn = spawnRate;

        EnemyController.HitCastle += HitCastle;
        EnemyController.EnemyDied += EnemyDied;
    }

    void Update()
    {
        if (_timeToNextSpawn < 0 && currentEnemies < maxEnemies)
        {
            _timeToNextSpawn = spawnRate;
            var newEnemy = Instantiate(enemyPrefab, _spawnPosition, Quaternion.identity,this.transform);
            currentEnemies++;
        }
        else
        {
            _timeToNextSpawn -= Time.deltaTime;
        }
    }

    private void OnDestroy()
    {
        EnemyController.HitCastle -= HitCastle;
        EnemyController.EnemyDied -= EnemyDied;
    }

    private void HitCastle(float f)
    {
        currentEnemies--;
    }
    private void EnemyDied(int guid)
    {
        currentEnemies--;
    }
}