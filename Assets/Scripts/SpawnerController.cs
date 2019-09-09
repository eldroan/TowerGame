using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SpawnerController : MonoBehaviour
{
    [SerializeField] private int maxEnemies;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnRate;
    private float _timeToNextSpawn;
    private Vector3 _spawnPosition;
    private int _currentEnemies;

    private void Awake()
    {
        //Assert.IsNotNull(enemyPrefab);
        _spawnPosition = this.transform.position;
        _timeToNextSpawn = spawnRate;
    }

    void Update()
    {
        if (_timeToNextSpawn < 0 && _currentEnemies < maxEnemies)
        {
            _timeToNextSpawn = spawnRate;
            var newEnemy = Instantiate(enemyPrefab, _spawnPosition, Quaternion.identity,this.transform);
            _currentEnemies++;
        }
        else
        {
            _timeToNextSpawn -= Time.deltaTime;
        }
    }
}