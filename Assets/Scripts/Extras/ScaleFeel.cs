using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleFeel : MonoBehaviour
{
    [SerializeField] private float resetSpeed = 6.0f;
    private Vector3 intialScale = Vector3.zero;
    private Vector3 targetScale = Vector3.zero;

    private void Start() {
        intialScale = transform.localScale;
    }

    private void LateUpdate() {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, resetSpeed * Time.deltaTime);
        targetScale = Vector3.Lerp(targetScale, intialScale, resetSpeed * Time.deltaTime);
    }

    public void ChangeScale(Vector3 scale) {
        targetScale += scale;
    }
}
