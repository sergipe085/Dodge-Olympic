using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour
{
    private JoystickButton button;
    public Vector2 position;

    private void Awake() {
        button = GetComponentInChildren<JoystickButton>();
    }

    private void Update() {
        position = (transform.position - button.transform.position).normalized;
    }
}
