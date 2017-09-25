using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class UGUIDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public delegate void PointerEventDataDelegate(GameObject go, PointerEventData eventData);

    public PointerEventDataDelegate onBeginDrag;
    public PointerEventDataDelegate onDrag;
    public PointerEventDataDelegate onEndDrag;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (onBeginDrag != null)
            onBeginDrag(gameObject, eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (onDrag != null)
            onDrag(gameObject, eventData);
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (onEndDrag != null)
            onEndDrag(gameObject, eventData);
    }

    public static UGUIDragHandler Get(GameObject go)
    {
        UGUIDragHandler listener = go.GetComponent<UGUIDragHandler>();
        if (listener == null)
            listener = go.AddComponent<UGUIDragHandler>();
        return listener;
    }

    public static UGUIDragHandler Get(Transform tran)
    {
        return Get(tran.gameObject);
    }
}
