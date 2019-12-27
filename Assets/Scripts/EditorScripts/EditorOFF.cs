using UnityEngine;
using UnityEngine.EventSystems;

public class EditorOFF : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IDragHandler, IEndDragHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        Editor.E.OFF[0] = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Editor.E.OFF[0] = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Editor.E.OFF[0] = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Editor.E.OFF[0] = false;
    }
}