using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoystickButton : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] private float outsideSize = 60.0f;
    private Vector2 initialPosition;

    private void Start() {
        initialPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos = eventData.position - initialPosition;
        pos = Vector2.ClampMagnitude(pos, outsideSize);
        transform.position = initialPosition + pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = new Vector2(0, 0);
        initialPosition = transform.position;
    }
}
