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
        _objectRigidBody.linearDamping = 5;
        _objectMeshCollider.enabled = false;
    }
    
    public void Drop()
    {
        this._objectGrabPointTransform = null;
        _objectRigidBody.isKinematic = false;
        _objectMeshCollider.enabled = true;
        _objectRigidBody.AddForce(_hand.forward * 100);
        
    }

    
    private float _sin = 0;
    private void FixedUpdate()
    {
        if (_objectGrabPointTransform != null)
        {/*
            _sin += Time.deltaTime * lerpSpeed;
            _sin = Mathf.Clamp(_sin, 0, Mathf.PI);
            float t = 0.5f * Mathf.Sin(_sin - Mathf.PI / 2f) + 0.5f;
            _objectRigidBody.transform.position = Vector3.Lerp(_objectRigidBody.transform.position, _objectGrabPointTransform.position, t);
*/
            Vector3 newPosition = Vector3.Lerp(transform.position, _objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
            _objectRigidBody.MovePosition(newPosition);
        }
    }
}