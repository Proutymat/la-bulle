using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeleportZone : MonoBehaviour
{
    [SerializeField] private Transform _destination;
    [SerializeField] private List<GameObject> _objectsToValidate = new List<GameObject>();
    private List<GameObject> _objectsInTheHole = new List<GameObject>();

    private void StoreObject(GameObject gameObject)
    {
        _objectsInTheHole.Add(gameObject);
        gameObject.SetActive(false);
    }

    private BubbleBehaviour _bubbleBehaviour;
    private void Start()
    {
        _bubbleBehaviour = GameObject.FindFirstObjectByType<BubbleBehaviour>();
    }

    private void PlayerInTheHole(Collider other)
    {
        // Teleport the player
        other.GetComponent<CharacterController>().enabled = false;
        other.transform.position = _destination.position;
        other.GetComponent<CharacterController>().enabled = true;
        
        // Teleport all objects to their origin position
        for(int i = 0; i < _objectsInTheHole.Count; i++)
        {
            _objectsInTheHole[i].SetActive(true);
            _objectsInTheHole[i].transform.position = _objectsInTheHole[i].GetComponent<GrabbableObject>().OriginPosition.position;
        }
        
        _objectsInTheHole.Clear();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>())
        {
            PlayerInTheHole(other);
        }
        else
        {
            StoreObject(other.gameObject);
            Debug.Log("Object in the hole");
            // Good objects in the hole
            if (_objectsInTheHole.Count == _objectsToValidate.Count && 
                !_objectsToValidate.Except(_objectsInTheHole).Any())
            {
                Debug.Log("Win");
                _bubbleBehaviour.OnWin();
            }
        }
    }
}
