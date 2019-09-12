using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    private float _fireRate;
    private float _damage;
    private bool _initialized;
    private List<Transform> _enemiesTransforms;
    private Transform _defaultPointToLook;

    private void Awake()
    {
        _initialized = false;
        _enemiesTransforms = new List<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_initialized) return;
        
        if (_enemiesTransforms.Count > 0 && _enemiesTransforms[0].gameObject.activeSelf)
        {
            this.transform.LookAt(_enemiesTransforms[0]);
        }
        else
        {
            this.transform.LookAt(_defaultPointToLook);
        }
    }

    public void Initialize(float damage, float fireRate, Transform defaultPointToLook)
    {
        _damage = damage;
        _fireRate = fireRate;
        _defaultPointToLook = defaultPointToLook;
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