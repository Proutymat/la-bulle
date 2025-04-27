using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeleportZone : MonoBehaviour
{
    [SerializeField] private Transform _destination;
    [SerializeField] private List<GameObject> _objectsToValidate = new List<GameObject>();
    private List<GameObject> _objectsInTheHole = new List<GameObject>();
    private CharacterController _controller;
    [SerializeField] private GameObject _reactifs;
    

    private void StoreObject(GameObject gameObject)
    {
        _objectsInTheHole.Add(gameObject);
        gameObject.SetActive(false);
    }

    private BubbleBehaviour _bubbleBehaviour;
    private void Start()
    {
        _bubbleBehaviour = GameObject.FindFirstObjectByType<BubbleBehaviour>();
        _controller = GameObject.FindFirstObjectByType<CharacterController>();
    }

    private void PlayerInTheHole(CharacterController chara)
    {
        Debug.Log("PLAYER IN THE HOLE");
        if (chara.gameObject.GetComponent<PlayerPickUp>().GrabbableObject)
        {
            GrabbableObject obj = chara.gameObject.GetComponent<PlayerPickUp>().DropObject();
            obj.gameObject.SetActive(true);
            obj.transform.position = obj.OriginPosition;
        }

        // Teleport the player
        chara.enabled = false;
        chara.transform.position = _destination.position;
        chara.enabled = true;

        // Activate and reset the objects
        for (int i = 0; i < _reactifs.transform.childCount; i++)
        {
            Transform child = _reactifs.transform.GetChild(i);
            child.gameObject.SetActive(true);

            GrabbableObject grabbable = child.GetComponent<GrabbableObject>();
            if (grabbable != null)
            {
                child.position = grabbable.OriginPosition;
            }
        }

        _objectsInTheHole.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>())
        {
            PlayerInTheHole(_controller);
        }
        else
        {
            Debug.Log("OBJECT IN THE HOLE");
            // Check if the object is already in the hole
            if(_objectsInTheHole.Contains(other.gameObject))
                return;
            StoreObject(other.gameObject);
            
            // Good objects in the hole
            if (_objectsInTheHole.Count == _objectsToValidate.Count &&
                !_objectsToValidate.Except(_objectsInTheHole).Any())
            {
                Debug.Log("Win");
                _bubbleBehaviour.OnWin();
                gameObject.SetActive(false);
            }
        }
    }
}