using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField] private GameObject target;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            this.transform.position = target.transform.position + new Vector3(-0.52f, 1.06202f, -1.611094f);
        }
    }
}
