using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    private Rigidbody _objectRigidBody;
    private MeshCollider _objectMeshCollider;
    private Transform _objectGrabPointTransform;
    [SerializeField] private float lerpSpeed = 10.0f;

    [SerializeField] private Transform _hand;	

    private void Awake()
    {
        _objectRigidBody = GetComponent<Rigidbody>();
        _objectMeshCollider = GetComponent<MeshCollider>();
    }
    public void Grab(Transform objectGrabPointTransform)
    {
        this._objectGrabPointTransform = objectGrabPointTransform;
        _objectRigidBody.isKinematic = true;
        _objectMeshCollider.enabled = false;
    }
    
    public void Drop()
    {
        this._objectGrabPointTransform = null;
        _objectRigidBody.isKinematic = false;
        _objectRigidBody.AddForce(_hand.forward * 100);
        _objectMeshCollider.enabled = true;
    }

    private void FixedUpdate()
    {
        if (_objectGrabPointTransform != null)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, _objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
            _objectRigidBody.MovePosition(newPosition);
        }
    }
}