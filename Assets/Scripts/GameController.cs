using System;
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
    [SerializeField] private GameObject selectedSpotWaypoint;
    [SerializeField] private GameObject turretShop;
    private Camera _mainCamera;
    private Transform _selectedSpot;

    // Start is called before the first frame update
    void Awake()
    {
        Assert.AreNotEqual(0f, startingCastleLife, "La vida no puede ser 0");
        _currentLife = startingCastleLife;
        EnemyController.HitCastle += HandleHit;
        _mainCamera = Camera.main;
        _selectedSpot = null;
        turretShop.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LayerMask layerMask = LayerMask.GetMask("TurretSpot");
            // Does the ray intersect any objects excluding the player layer
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, layerMask))
            {
//                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Debug.Log($"Did Hit{hit.transform.gameObject.name}");
                _selectedSpot = hit.transform;
                
                //Pongo el waypoint
                selectedSpotWaypoint.SetActive(true);
                selectedSpotWaypoint.transform.parent = _selectedSpot;
                selectedSpotWaypoint.transform.localPosition = Vector3.zero;
                turretShop.SetActive(true);
            }
            else
            {
                turretShop.SetActive(false);
                selectedSpotWaypoint.SetActive(false);
                _selectedSpot = null;
            }
        }
    }

    void HandleHit(float damage)
    {
        _currentLife -= damage;
        healthBarGameObject.transform.localScale = new Vector3(_currentLife / startingCastleLife, 1, 1);
    }
}