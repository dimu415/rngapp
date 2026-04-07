using UnityEngine;
using UnityEngine.EventSystems;
public class TouchArea : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler
{
    public TouchRandomSelector selector;

    public void OnPointerDown(PointerEventData eventData)
    {
        selector.OnTouchDown(eventData.pointerId, eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        selector.OnTouchUp(eventData.pointerId);
    }
}
