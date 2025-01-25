using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    public void Interact(PlayerPickUp interactor);
}

public class PlayerPickUp : MonoBehaviour
{
    [SerializeField] private Transform _playerCameraTransform;
    [SerializeField] private Transform _objectGrabPointTransform;
    [SerializeField] private LayerMask _pickUpLayerMask;
    [SerializeField] private float _pickUpDistance = 2.0f;
    
    // PRESS E VARIABLES
    [SerializeField] private Canvas _pressECanvas;
    
    private GrabbableObject _grabbableObject;
    
    // Update is called once per frame
    void Update()
    {
        bool displayCanvas = false;

        // we dont grab anything and we can grab something
        if (_grabbableObject == null)
        {
            if (Physics.Raycast(_playerCameraTransform.position, _playerCameraTransform.forward,
                    out RaycastHit raycastHit, _pickUpDistance, _pickUpLayerMask))
            {
                // we can grab something
                if (raycastHit.transform.TryGetComponent(out _grabbableObject))
                {
                    displayCanvas = true;

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        _grabbableObject.Grab(_objectGrabPointTransform);
                        
                    }
                    else
                    {
                        _grabbableObject = null; // very quick and dirty solution (don't judge)
                    }
                }
                // we can interact with something
                else if (raycastHit.collider.TryGetComponent(out IInteractable interactObject))
                {
                    displayCanvas = true;

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        interactObject.Interact(this);
                    }
                    else
                    {
                        interactObject = null;
                    }
                }
            }
        }
        // we are grabbing something
        else if (Input.GetKeyDown(KeyCode.E))
        {
            _grabbableObject.Drop();
            _grabbableObject = null;
            //La son drop
        }

        // show press E canvas
        if (displayCanvas)
        {
            _pressECanvas.gameObject.SetActive(true);
        }
        // don't show press E canvas
        else
        {
            _pressECanvas.gameObject.SetActive(false);
        }
    }
}