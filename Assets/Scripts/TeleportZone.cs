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
        // Teleport the player
        chara.enabled = false;
        chara.transform.position = _destination.position;
        chara.enabled = true;

        // Teleport all objects to their origin position
        for (int i = 0; i < _objectsInTheHole.Count; i++)
        {
            _objectsInTheHole[i].SetActive(true);
            _objectsInTheHole[i].transform.position = _objectsInTheHole[i].GetComponent<GrabbableObject>().OriginPosition;
        }

        _objectsInTheHole.Clear();
    }

    private void Update()
    {
        if(_controller.transform.position.y <= transform.position.y)
        {
            PlayerInTheHole(_controller);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>())
        {
            return;
        }
        else
        {
            if(_objectsInTheHole.Contains(other.gameObject))
                return;
            StoreObject(other.gameObject);
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