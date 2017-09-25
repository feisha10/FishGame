using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public class UIEventListener : UnityEngine.EventSystems.EventTrigger
{
    public delegate void VoidDelegate(GameObject go);
    public delegate void BoolDelegate(GameObject go, bool state);
    public delegate void BoolDelegateData(GameObject go, bool state, PointerEventData eventData);
    public delegate void FloatDelegate(GameObject go, float delta);
    public delegate void VectorDelegate(GameObject go, Vector2 delta, int state);   //状态0，开始1结束
    public delegate void ObjectDelegate(GameObject go, GameObject draggedObject);
    public delegate void KeyCodeDelegate(GameObject go, KeyCode key);

    public VoidDelegate onSubmit;
    public VoidDelegate onClick;
    public VoidDelegate onDoubleClick;
    public BoolDelegate onHover;
    public BoolDelegate onPress;
    public BoolDelegateData onPressData;
    public BoolDelegate onSelect;
    public FloatDelegate onScroll;
    public VectorDelegate onDrag;
    public ObjectDelegate onDrop;
    public KeyCodeDelegate onKey;

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        
        if (onClick != null)
        {
            onClick(gameObject);
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (onPress != null) onPress(gameObject, true);
        if (onPressData != null) onPressData(gameObject, true, eventData);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if (onHover != null) onHover(gameObject, true);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        
        if (onHover != null) onHover(gameObject, false);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        
        if (onPress != null) onPress(gameObject, false);
        if (onPressData != null) onPressData(gameObject, false, eventData);
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        
        if (onSelect != null) onSelect(gameObject,true);
    }

    public override void OnUpdateSelected(BaseEventData eventData)
    {
        base.OnUpdateSelected(eventData);
        
        if (onSelect != null) onSelect(gameObject,false);
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);

        if (onDrag != null) onDrag(gameObject, eventData.delta, 0);
    }
    
    public override void OnCancel(BaseEventData eventData)
    {
        base.OnCancel(eventData);  
    }
   
    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);       
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        if (onDrag != null) onDrag(gameObject, eventData.delta, 2);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        if (onDrag != null) onDrag(gameObject, eventData.delta, 1);
    }
   
    public override void OnDrop(PointerEventData eventData)
    {
        base.OnDrop(eventData);
        
        if (onDrop != null) onDrop(gameObject, eventData.pointerDrag);
    }
    
    public override void OnInitializePotentialDrag(PointerEventData eventData)
    {
        base.OnInitializePotentialDrag(eventData);      
    }
    
    public override void OnMove(AxisEventData eventData)
    {
        base.OnMove(eventData);       
    }
    
    public override void OnScroll(PointerEventData eventData)
    {
        base.OnScroll(eventData);    
    }
    
    public override void OnSubmit(BaseEventData eventData)
    {
        base.OnSubmit(eventData);
    }
    
    /// <summary>
    /// Get or add an event listener to the specified game object.
    /// </summary>

    static public UIEventListener Get(GameObject go)
    {
        UIEventListener listener = go.GetComponent<UIEventListener>();
        if (listener == null) listener = go.AddComponent<UIEventListener>();
        return listener;
    }

    static public UIEventListener Get(GameObject go,string sound)
    {
        UIEventListener listener = Get(go);
        return listener;
    }

}
