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
    public static Action<int> EnemyDied = delegate { };

    // Update is called once per frame

    private void Awake()
    {
        _currentLife = maxLife;
    }

    private void Update()
    {
        this.transform.Translate(Time.deltaTime * speed * Vector3.left);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            var bulletGo = other.gameObject;
            var inflictedDamage = bulletGo.GetComponent<Bullet>().Damage;
            _currentLife -= inflictedDamage;
            
            Destroy(bulletGo); //Esto estaria bueno reutilizar con un sistema de pooling porque instanciar y destruir es malisimo para la performance

            if (_currentLife <= 0)
            {
                EnemyDied(this.transform.GetInstanceID());
                Destroy(this.gameObject);
                return;
            }
        }
        
        if (other.CompareTag("Castle"))
        {
            Debug.Log("Auch, estaba re duro ese castillo");
            HitCastle(damage);
            
            Destroy(this.gameObject);
        }
    }
}
