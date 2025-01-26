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

    [Header("Sounds")]
    [SerializeField] private List<AudioClip> cassette;
    // PRESS E VARIABLES
    [SerializeField] private Canvas _pressECanvas;
    
    [SerializeField] private GrabbableObject _grabbableObject;
    [SerializeField] private GameObject _magnetoscope;
    
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
                    AudioSource audioSourceObject;
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        _grabbableObject.Grab(_objectGrabPointTransform);
                        audioSourceObject = _grabbableObject.GetComponent<AudioSource>(); //Là recuperer
                        audioSourceObject.volume = Random.Range(0.75f, 0.95f); 
                        audioSourceObject.pitch = Random.Range(0.8f, 1f);
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
        _magnetoscope.GetComponent<Magnetophone>().SetTape(_grabbableObject.gameObject);
        _grabbableObject = null;
    }

    private void HandleInteractiblesObjects()
    {
        AudioSource audioSourceMagnetophone;
        Animator animatorMagnetophone;
        // Is the player looking at a magnetophone
        if (DetectMagnetophone())
        {
            if (_grabbableObject == null)
            {
                Debug.Log("No object in hand");
                animatorMagnetophone = _magnetoscope.GetComponent<Animator>();
                animatorMagnetophone.SetTrigger("hasError");
                audioSourceMagnetophone = _magnetoscope.GetComponent<AudioSource>();
                audioSourceMagnetophone.PlayOneShot(cassette[0]);
            }
            else
            {
                if (_grabbableObject.ObjectName == "Tape1")
                {
                    Debug.Log("Playing tape 1");
                    animatorMagnetophone = _magnetoscope.GetComponent<Animator>();
                    animatorMagnetophone.SetTrigger("hasTape");
                    audioSourceMagnetophone = _magnetoscope.GetComponent<AudioSource>();
                    audioSourceMagnetophone.PlayOneShot(cassette[1]);
                    
                    ConsumeObject();
                    StartCoroutine(WaitForClipToEnd(cassette[1].length));
                }
                else if (_grabbableObject.ObjectName == "Tape2")
                {
                    animatorMagnetophone = _magnetoscope.GetComponent<Animator>();
                    animatorMagnetophone.SetTrigger("hasTape");
                    audioSourceMagnetophone = _magnetoscope.GetComponent<AudioSource>();
                    audioSourceMagnetophone.PlayOneShot(cassette[2]);
                    ConsumeObject();
                    StartCoroutine(WaitForClipToEnd(cassette[2].length));
                    
                }
                else if (_grabbableObject.ObjectName == "Tape3")
                {
                    animatorMagnetophone = _magnetoscope.GetComponent<Animator>();
                    animatorMagnetophone.SetTrigger("hasTape");
                    audioSourceMagnetophone = _magnetoscope.GetComponent<AudioSource>();
                    audioSourceMagnetophone.PlayOneShot(cassette[3]);
                    ConsumeObject();
                    StartCoroutine(WaitForClipToEnd(cassette[3].length));
                }
                else if (_grabbableObject.ObjectName == "Tape4")
                {
                    animatorMagnetophone = _magnetoscope.GetComponent<Animator>();
                    animatorMagnetophone.SetTrigger("hasTape");
                    audioSourceMagnetophone = _magnetoscope.GetComponent<AudioSource>();
                    audioSourceMagnetophone.PlayOneShot(cassette[4]);
                    ConsumeObject();
                    StartCoroutine(WaitForClipToEnd(cassette[4].length));
                }
                else if (_grabbableObject.ObjectName == "Intro")
                {
                    animatorMagnetophone = _magnetoscope.GetComponent<Animator>();
                    animatorMagnetophone.SetTrigger("hasTape");
                    audioSourceMagnetophone = _magnetoscope.GetComponent<AudioSource>();
                    audioSourceMagnetophone.PlayOneShot(cassette[5]);
                    ConsumeObject();
                    StartCoroutine(WaitForClipToEnd(cassette[5].length));
                }
                else if (_grabbableObject.ObjectName == "End")
                {
                    animatorMagnetophone = _magnetoscope.GetComponent<Animator>();
                    animatorMagnetophone.SetTrigger("hasTape");
                    audioSourceMagnetophone = _magnetoscope.GetComponent<AudioSource>();
                    audioSourceMagnetophone.PlayOneShot(cassette[6]);
                    ConsumeObject();
                    StartCoroutine(WaitForClipToEnd(cassette[6].length));
                }
                else
                {
                    animatorMagnetophone = _magnetoscope.GetComponent<Animator>();
                    animatorMagnetophone.SetTrigger("hasError");
                    audioSourceMagnetophone = _magnetoscope.GetComponent<AudioSource>();
                    audioSourceMagnetophone.PlayOneShot(cassette[0]);
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
    
    private IEnumerator WaitForClipToEnd(float duration)
    {
        Animator animatorMagnetophone;
        animatorMagnetophone = _magnetoscope.GetComponent<Animator>();
        yield return new WaitForSeconds(duration);
        animatorMagnetophone.SetTrigger("hasFinishAudio");
    }
}