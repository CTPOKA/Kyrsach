using UnityEngine;
using UnityEngine.EventSystems;

public class drag : MonoBehaviour, IDragHandler, IPointerEnterHandler
{
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.SetSiblingIndex(99);
    }
}
