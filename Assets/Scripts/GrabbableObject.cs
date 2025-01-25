using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GrabbableObject : MonoBehaviour
{
    private Rigidbody _objectRigidBody;
    private Transform _objectGrabPointTransform;
    [SerializeField] private float lerpSpeed = 10.0f;
    [SerializeField] private Transform _hand;
    
    [SerializeField] private Transform _originPosition;
    [SerializeField] private string _objectName;

    public Transform OriginPosition { get { return _originPosition; } }
    public string ObjectName { get { return _objectName; } }
    
    [Header("Sounds")]
    public AudioClip grabObjSound;
    [SerializeField] private AudioClip dropObjSound;
    private AudioSource audioSourceObject;

    private void Awake()
    {
        _objectRigidBody = GetComponent<Rigidbody>();
        audioSourceObject = GetComponent<AudioSource>();
    }
    public void Grab(Transform objectGrabPointTransform)
    {
        this._objectGrabPointTransform = objectGrabPointTransform;
        _objectRigidBody.isKinematic = true;
    }
    
    public void Drop()
    {
        this._objectGrabPointTransform = null;
        _objectRigidBody.isKinematic = false;
        _objectRigidBody.AddForce(_hand.forward * 150);
        
    }
    private void FixedUpdate()
    {
        if (_objectGrabPointTransform != null)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, _objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
            _objectRigidBody.MovePosition(newPosition);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        audioSourceObject.volume = Random.Range(0.75f, 0.10f); 
        audioSourceObject.pitch = Random.Range(0.8f, 1.2f);
        audioSourceObject.PlayOneShot(dropObjSound);
    }
}