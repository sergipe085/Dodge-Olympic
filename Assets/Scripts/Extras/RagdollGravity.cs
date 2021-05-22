using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollGravity : MonoBehaviour
{
    [SerializeField] private float force = 10.0f;
    private Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (rigidbody)
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, -force, rigidbody.velocity.z);
        }
    }
}
