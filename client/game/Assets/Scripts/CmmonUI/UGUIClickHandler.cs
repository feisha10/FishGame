using UnityEngine;
using UnityEngine.EventSystems;

class UGUIClickHandler: MonoBehaviour, IPointerClickHandler
{
    //public delegate void PointerEvetCallBackFunc(GameObject target);
    public  UIEventListener.VoidDelegate onPointerClick;
    public  UIEventListener.VoidDelegate onDoublePointerClick;

    public void  OnPointerClick(PointerEventData eventData)
    {
        if (gameObject == null)
            return;

        if (onDoublePointerClick != null && eventData.clickCount == 2)
            onDoublePointerClick(gameObject);
        else if (onPointerClick != null)
        {
            onPointerClick(gameObject);
        }
    }

    public bool IsEmpty()
    {
        return onPointerClick == null;
    }

    public void RemoveAllHandler()
    {
        onPointerClick = null;
        onDoublePointerClick = null;
        DestroyImmediate(this);
    }

    public static UGUIClickHandler Get(GameObject go)
    {
        UGUIClickHandler listener = go.GetComponent<UGUIClickHandler>();
        if (listener == null) 
            listener = go.AddComponent<UGUIClickHandler>();
        return listener;
    }

    public static UGUIClickHandler Get(Transform tran)
    {
        return Get(tran.gameObject);
    }
}
