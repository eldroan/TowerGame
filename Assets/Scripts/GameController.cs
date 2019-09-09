using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameController : MonoBehaviour
{
    [SerializeField] private float startingCastleLife;
    private float _currentLife;
    [SerializeField] private int gems;

    [SerializeField] private GameObject healthBarGameObject;

        // Start is called before the first frame update
    void Awake()
    {
        Assert.AreNotEqual(0f,startingCastleLife,"La vida no puede ser 0");
        _currentLife = startingCastleLife;
        EnemyController.HitCastle += HandleHit;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HandleHit(float damage)
    {
        _currentLife -= damage;
        healthBarGameObject.transform.localScale = new Vector3(_currentLife/startingCastleLife,1,1);
    }
}
