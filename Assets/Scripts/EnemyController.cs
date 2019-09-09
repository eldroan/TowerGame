using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Time.deltaTime * speed * Vector3.left);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Castle"))
        {
            Debug.Log("Auch, estaba re duro ese castillo");   
        }
    }
}
