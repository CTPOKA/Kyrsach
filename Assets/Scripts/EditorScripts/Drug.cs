using UnityEngine;
using UnityEngine.EventSystems;

public class Drug : MonoBehaviour, IEventSystemHandler, IDragHandler, IPointerClickHandler
{
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        transform.SetSiblingIndex(99);
    }
}
