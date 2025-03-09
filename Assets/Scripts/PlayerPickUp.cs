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

    private bool _canDrop;
    
    private bool _hasAlreadyInteractedOnce;
    
    private void Start()
    {
        _hasAlreadyInteractedOnce = false;
        _canDrop = false;
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
        else if (Input.GetKeyDown(KeyCode.E) && _canDrop)
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
                _canDrop = false;
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
            if (_magnetoscope.GetComponent<Magnetophone>().currentTape == null)
            {

                // No object in hand, play error animation
                if (_grabbableObject == null)
                {
                    Debug.Log("No object in hand");
                    animatorMagnetophone = _magnetoscope.GetComponent<Animator>();
                    animatorMagnetophone.SetTrigger("hasError");
                    audioSourceMagnetophone = _magnetoscope.GetComponent<AudioSource>();
                    audioSourceMagnetophone.PlayOneShot(cassette[0]);
                }
                // Object in hand
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
                        Debug.Log("Playing tape 2");
                        animatorMagnetophone = _magnetoscope.GetComponent<Animator>();
                        animatorMagnetophone.SetTrigger("hasTape");
                        audioSourceMagnetophone = _magnetoscope.GetComponent<AudioSource>();
                        audioSourceMagnetophone.PlayOneShot(cassette[2]);
                        ConsumeObject();
                        StartCoroutine(WaitForClipToEnd(cassette[2].length));

                    }
                    else if (_grabbableObject.ObjectName == "Tape3")
                    {
                        Debug.Log("Playing tape 3");
                        animatorMagnetophone = _magnetoscope.GetComponent<Animator>();
                        animatorMagnetophone.SetTrigger("hasTape");
                        audioSourceMagnetophone = _magnetoscope.GetComponent<AudioSource>();
                        audioSourceMagnetophone.PlayOneShot(cassette[3]);
                        ConsumeObject();
                        StartCoroutine(WaitForClipToEnd(cassette[3].length));
                    }
                    else if (_grabbableObject.ObjectName == "Tape4")
                    {
                        Debug.Log("Playing tape 4");
                        animatorMagnetophone = _magnetoscope.GetComponent<Animator>();
                        animatorMagnetophone.SetTrigger("hasTape");
                        audioSourceMagnetophone = _magnetoscope.GetComponent<AudioSource>();
                        audioSourceMagnetophone.PlayOneShot(cassette[4]);
                        ConsumeObject();
                        StartCoroutine(WaitForClipToEnd(cassette[4].length));
                    }
                    else if (_grabbableObject.ObjectName == "Intro")
                    {
                        Debug.Log("Playing intro tape");
                        animatorMagnetophone = _magnetoscope.GetComponent<Animator>();
                        animatorMagnetophone.SetTrigger("hasTape");
                        audioSourceMagnetophone = _magnetoscope.GetComponent<AudioSource>();
                        audioSourceMagnetophone.PlayOneShot(cassette[5]);
                        ConsumeObject();
                        StartCoroutine(WaitForClipToEnd(cassette[5].length));
                    }
                    else if (_grabbableObject.ObjectName == "End")
                    {
                        Debug.Log("Playing end tape");
                        animatorMagnetophone = _magnetoscope.GetComponent<Animator>();
                        animatorMagnetophone.SetTrigger("hasTape");
                        audioSourceMagnetophone = _magnetoscope.GetComponent<AudioSource>();
                        audioSourceMagnetophone.PlayOneShot(cassette[6]);
                        ConsumeObject();
                        StartCoroutine(WaitForClipToEnd(cassette[6].length));
                    }
                    else if (_grabbableObject.ObjectName == "Tuto")
                    {
                        Debug.Log("Playing Tuto tape");
                        animatorMagnetophone = _magnetoscope.GetComponent<Animator>();
                        animatorMagnetophone.SetTrigger("hasTape");
                        audioSourceMagnetophone = _magnetoscope.GetComponent<AudioSource>();
                        audioSourceMagnetophone.PlayOneShot(cassette[7]);
                        ConsumeObject();
                        StartCoroutine(WaitForClipToEnd(cassette[7].length));
                    }
                    // Grabbed object is not a tape
                    else
                    {
                        Debug.Log("Grabbed object is not a tape!");
                        audioSourceMagnetophone = _magnetoscope.GetComponent<AudioSource>();
                        audioSourceMagnetophone.PlayOneShot(cassette[0]);
                        //DropObject();
                    }
                }
            }
            // Magnetophone already has a tape
            else
            {
                audioSourceMagnetophone = _magnetoscope.GetComponent<AudioSource>();
                audioSourceMagnetophone.PlayOneShot(cassette[0]);
                Debug.Log("PlayerPickUp: Magnetoscope has already a tape!");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        _canDrop = true;
        Debug.Log("currentTape = " + _magnetoscope.GetComponent<Magnetophone>().currentTape);

        // Is the plauer trying to interact with something
        if (Input.GetKeyDown(KeyCode.E))
        {
            HandleInteractiblesObjects();
        }
        
        HandleGrabbableObjects();
    }
    
    private IEnumerator WaitForClipToEnd(float duration)
    {
        Debug.Log("PlayerPickUp: waiting = " + duration);
        Animator animatorMagnetophone;
        animatorMagnetophone = _magnetoscope.GetComponent<Animator>();
        yield return new WaitForSeconds(duration);
        animatorMagnetophone.SetTrigger("hasFinishAudio");
        //_magnetoscope.GetComponent<Magnetophone>().Drop();
    }
}