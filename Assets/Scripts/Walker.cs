using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour
{
    public bool isStuck;

    private Rigidbody _rigidbody;
    private MeshRenderer _meshRenderer;
    private Vector3 _currentVelocity;
    private float _currentColorHue;

    public bool IsStuck => isStuck;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _meshRenderer = transform.GetComponent<MeshRenderer>();
        _meshRenderer.material = new Material(_meshRenderer.material);
    }

    private void Start()
    {
        if (isStuck)
        {
            _rigidbody.isKinematic = true;
            _meshRenderer.material.color = Color.HSVToRGB(WalkerManager.Instance.particleHue, 1f, Mathf.Min(1f, 0.1f + transform.position.magnitude * 0.05f));
            WalkerManager.Instance.RecalculateStructureRadius();
        }
    }

    private void Update()
    {
        if (IsStuck && !Mathf.Approximately(_currentColorHue, WalkerManager.Instance.particleHue))
        {
            _meshRenderer.material.color = Color.HSVToRGB(WalkerManager.Instance.particleHue, 1f, Mathf.Min(1f, 0.1f + transform.position.magnitude * 0.05f));
            _currentColorHue = WalkerManager.Instance.particleHue;
        }
    }

    private void FixedUpdate()
    {
        if (!IsStuck)
        {
            Vector3 targetVelocity = Vector3.zero;
        
            Vector3 randomVelocityDir = VectorUtils.RandomVectorOnSphere(Vector3.zero, 1f);
            targetVelocity += randomVelocityDir;
        
            float distanceFromCenter = Vector3.Distance(Vector3.zero, transform.position);
            if (distanceFromCenter > WalkerManager.Instance.maxWalkerCenterDistance)
            {
                Vector3 restoringForce = (-1f * transform.position).normalized * WalkerManager.Instance.restoringForcePower;
                targetVelocity += restoringForce;
            }

            targetVelocity = targetVelocity.normalized * WalkerManager.Instance.maxSpeed;
            PhysicsUtils.AddForceToReachVelocity(_rigidbody, targetVelocity, WalkerManager.Instance.forceMagnitude);
        }
    }

    private void Stick(Transform stuckWalker)
    {
        if (!isStuck)
        {
            isStuck = true;
            _rigidbody.isKinematic = true;
            if (stuckWalker != null)
            {
                transform.position = stuckWalker.position +
                                     (transform.position - stuckWalker.position).normalized * transform.localScale.x;
            }
            _meshRenderer.material.color = Color.HSVToRGB(WalkerManager.Instance.particleHue, 1f, Mathf.Min(1f, 0.1f + transform.position.magnitude * 0.05f));
            transform.parent = GameObject.FindWithTag("StructureRoot").transform;
            WalkerManager.Instance.RecalculateStructureRadius();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Walker collidedWalker = other.transform.GetComponent<Walker>();
        if (collidedWalker != null && collidedWalker.IsStuck)
        {
            Stick(collidedWalker.transform);
        }
    }
}
