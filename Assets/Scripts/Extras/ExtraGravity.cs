using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraGravity : MonoBehaviour
{
    [SerializeField] private float force = 10.0f;
    private Rigidbody rigidbody;

    private void Start() {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        if (rigidbody) {
            rigidbody.AddForce(Vector3.down * force, ForceMode.Force);
        }
    }
}
