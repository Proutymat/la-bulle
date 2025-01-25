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
    [SerializeField] private float _interactingDistance = 2.0f;

    
    // PRESS E VARIABLES
    [SerializeField] private Canvas _pressECanvas;
    
    private GrabbableObject _grabbableObject;

    private bool _hasAlreadyInteractedOnce;
    
    private void Start()
    {
        _hasAlreadyInteractedOnce = false;
    }

    private void DropObject()
    {
        _grabbableObject.Drop();
        _grabbableObject = null;
    }

    private void HandleGrabbableObjects()
    {
        bool displayCanvas = false;
        AudioSource audioSourceObject;
        

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
                        audioSourceObject = _grabbableObject.GetComponent<AudioSource>(); //Là recuperer
                        audioSourceObject.PlayOneShot(_grabbableObject.grabObjSound);
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
        // we are grabbing something and we can drop it
        else if (Input.GetKeyDown(KeyCode.E))
        {
            DropObject();
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
    
    private bool DetectMagnetophone()
    {
        RaycastHit hit;
        if (Physics.Raycast(_playerCameraTransform.position, _playerCameraTransform.forward, out hit, _interactingDistance))
        {
            if (hit.collider.gameObject.GetComponent<Magnetophone>())
            {
                
                return true;
            }
        }
        
        return false;
    }

    private void ConsumeObject()
    {
        _grabbableObject.gameObject.SetActive(false);
        _grabbableObject = null;
    }

    private void HandleInteractiblesObjects()
    {
        // Is the player looking at a magnetophone
        if (DetectMagnetophone())
        {
            if (_grabbableObject == null)
            {
                Debug.Log("No object in hand");
                // PLAY EMPTY MAGNETOPHONE SOUND (antoine)
            }
            else
            {
                if (_grabbableObject.ObjectName == "Tape1")
                {
                    // SON CASSETTE 1
                    ConsumeObject();
                }
                else if (_grabbableObject.ObjectName == "Tape2")
                {
                    // SON CASSETTE 2
                    ConsumeObject();
                }
                else if (_grabbableObject.ObjectName == "Tape3")
                {
                    // SON CASSETTE 3
                    ConsumeObject();
                }
                else if (_grabbableObject.ObjectName == "Tape4")
                {
                    // SON CASSETTE 4
                    ConsumeObject();
                }
                else
                {
                    Debug.Log("You can't interact with this object");
                    DropObject();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Is the plauer trying to interact with something
        if (Input.GetKeyDown(KeyCode.E))
        {
            HandleInteractiblesObjects();
        }
        
        HandleGrabbableObjects();
    }
}