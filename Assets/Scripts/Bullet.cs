using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    private Transform _target = null;
    private bool _readyToGo = false;
    public float Damage { get; private set; }

    private void FixedUpdate()
    {
        if (_readyToGo)
        {
            this.transform.LookAt(_target);
            this.transform.Translate(Vector3.forward * speed);
        }
    }

    public void Initialize(float damage, Transform target)
    {
        Damage = damage;
        _target = target;
        _readyToGo = true;
    }
}
