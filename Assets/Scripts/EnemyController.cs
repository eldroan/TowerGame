using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private float maxLife;
    private float _currentLife;
    // Start is called before the first frame update
    
    public static Action<float> HitCastle = delegate(float f) {  };
    public static Action EnemyDied = delegate { };

    // Update is called once per frame

    private void Awake()
    {
        _currentLife = maxLife;
    }

    void Update()
    {
        this.transform.Translate(Time.deltaTime * speed * Vector3.left);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Castle"))
        {
            Debug.Log("Auch, estaba re duro ese castillo");
            HitCastle(damage);
            
            //Me destruyo (aunque me deberia poolear para mejorar performance)
            Destroy(this.gameObject);
        }

        if (other.CompareTag("Bullet"))
        {
            var inflictedDamage = other.gameObject.GetComponent<Bullet>().damage;
            _currentLife -= inflictedDamage;

            if (_currentLife <= 0)
                EnemyDied();
        }
    }
}
