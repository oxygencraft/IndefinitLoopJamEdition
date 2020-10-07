using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TaskListMonitor : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private UnityEvent clickEvent = new UnityEvent();

    [SerializeField]
    private UnityEvent enterEvent = new UnityEvent();

    [SerializeField]
    private UnityEvent exitEvent = new UnityEvent();

    public void OnPointerClick(PointerEventData eventData)
    {
        clickEvent.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        enterEvent.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        exitEvent.Invoke();
    }
}