using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    private float _fireRate;
    private float _secondsSinceLastFired;
    private float _damage;
    private bool _initialized;
    private List<Transform> _enemiesTransforms;
    private Transform _defaultPointToLook;
    private GameObject _bulletPrefab;

    private void Awake()
    {
        _initialized = false;
        _secondsSinceLastFired = 0f;
        _enemiesTransforms = new List<Transform>();
        EnemyController.EnemyDied += EnemyDied;

    }

    private void EnemyDied(int guid)
    {
        //Solucion anti performance, devuelve una lista nueva sin el nemeigo que murio si es que lo tenia
        _enemiesTransforms = _enemiesTransforms.Where(e => e.GetInstanceID() != guid).ToList(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (!_initialized) return;
        
        _secondsSinceLastFired += Time.deltaTime; //Time deltatime devuelve el tiempo en segundos desde el ultimo frame, es una forma de contar tiempo con framerates variables


        if (_enemiesTransforms.Count > 0 && _enemiesTransforms[0].gameObject.activeSelf)
        {
            this.transform.LookAt(_enemiesTransforms[0]);
            
            if (_secondsSinceLastFired > _fireRate)
            {
                //Tengo que disparar
                var newBullet = Instantiate(_bulletPrefab, this.transform.position, Quaternion.identity)
                    .GetComponent<Bullet>();
            
                newBullet.Initialize(_damage,_enemiesTransforms[0]); //Luego de la inicializacion la balla va a ir a la nave hasta chocar
                _secondsSinceLastFired = 0;
            }
        }
        else
        {
            this.transform.LookAt(_defaultPointToLook);
        }
    }

    private void OnDestroy()
    {
        EnemyController.EnemyDied -= EnemyDied;

    }

    public void Initialize(float damage, float fireRate, Transform defaultPointToLook, GameObject bulletPrefab)
    {
        _damage = damage;
        _fireRate = fireRate;
        _defaultPointToLook = defaultPointToLook;
        _bulletPrefab = bulletPrefab;
        _initialized = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            //Es un enemigo, lo anoto
            _enemiesTransforms.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            //Es un enemigo, lo saco
            _enemiesTransforms.Remove(other.transform);
        }
    }
}