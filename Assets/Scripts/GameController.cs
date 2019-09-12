using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class GameController : MonoBehaviour
{
    [SerializeField] private float startingCastleLife;
    private float _currentLife;
    [SerializeField] private int gems;

    [SerializeField] private GameObject healthBarGameObject;
    [SerializeField] private GameObject selectedSpotWaypoint;
    [SerializeField] private GameObject turretShop;
    [SerializeField] private List<TurretItems> turrets;
    [SerializeField] private Transform pointWhereSpawningTurretsShouldLookTo; //Nombrecito eh

    private Camera _mainCamera;
    private int _selectedSpot;
    private Dictionary<int, TurretSpot> _turretSpots;

    // Start is called before the first frame update
    void Awake()
    {
        _turretSpots = new Dictionary<int, TurretSpot>();
        Assert.AreNotEqual(0f, startingCastleLife, "La vida no puede ser 0");
        _currentLife = startingCastleLife;
        EnemyController.HitCastle += HandleHit;
        _mainCamera = Camera.main;
        _selectedSpot = -1;
        turretShop.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LayerMask layerMask = LayerMask.GetMask("TurretSpot");
            // Does the ray intersect any objects excluding the player layer
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (!Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) return; //Si no le pego a nada corto aca
            
            Debug.Log($"Did Hit{hit.transform.gameObject.name}");

            var hitTransform = hit.transform;
            var newSelectedSpot = hitTransform.GetInstanceID(); //getinstanceID es unico entre objetos, si vienen 2 iguales son el mismo objeto

            Debug.LogAssertion(newSelectedSpot != _selectedSpot);
            
            //si newSelectedSpot no esta en mi lista de lugares lo agrego
            if (_turretSpots.ContainsKey(newSelectedSpot) == false)
                _turretSpots.Add(newSelectedSpot, new TurretSpot(hitTransform));
            
            if (newSelectedSpot != _selectedSpot && _turretSpots[newSelectedSpot].containsTurret == false)
            {
                //Si no hay torreta en ese spot levanto el waypoint sino no
                _selectedSpot = newSelectedSpot;
                selectedSpotWaypoint.SetActive(true);
                selectedSpotWaypoint.transform.parent = hitTransform;
                selectedSpotWaypoint.transform.localPosition = Vector3.zero;
                turretShop.SetActive(true);
            }
            else
            {
                turretShop.SetActive(false);
                selectedSpotWaypoint.SetActive(false);
                _selectedSpot = -1;
            }
        }
    }

    void HandleHit(float damage)
    {
        _currentLife -= damage;
        healthBarGameObject.transform.localScale = new Vector3(_currentLife / startingCastleLife, 1, 1);
    }

    public void TryToPurchaseTurret(int turretNumber)
    {
        if (turretNumber > turrets.Count)
        {
            Debug.Log("Numero mayor a cantidad");
            return;
        }

        var desiredTurret = turrets[turretNumber];

        if (desiredTurret.gemPrice > gems)
        {
            Debug.Log("No te alcanza :c");
            return;
        }

        if (_turretSpots.TryGetValue(_selectedSpot, out var turretSpot))
        {
            if (turretSpot.containsTurret)
            {
                Debug.Log("No se puede crear porque ya existe una torre alli");
            }
            else
            {
                gems -= desiredTurret.gemPrice;
                var newTurret = Instantiate(desiredTurret.turretPrefab, turretSpot.turretSpotTransform.position, Quaternion.identity,
                    turretSpot.turretSpotTransform);
                newTurret.transform.LookAt(pointWhereSpawningTurretsShouldLookTo);
                turretSpot.containsTurret = true;
                selectedSpotWaypoint.SetActive(false); //Saco el waypoint para que no se vea
                turretShop.SetActive(false); //cierro el shop

            }
        }
        else
        {
            Debug.Log("No se encontro el lugar seleccionado");
        }
        
    }
}

[Serializable]
public class TurretItems
{
    public int gemPrice;
    public GameObject turretPrefab;
}

[Serializable]
public class TurretSpot
{
    
    public TurretSpot(Transform t)
    {
        containsTurret = false;
        turretSpotTransform = t;
    }
    public bool containsTurret;
    public Transform turretSpotTransform;
}