using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IDragHandler, IEventSystemHandler, IPointerEnterHandler
{
    public void OnDrag(PointerEventData eventData)
    {
        base.transform.position = Input.mousePosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        base.transform.SetSiblingIndex(99);
    }
}
