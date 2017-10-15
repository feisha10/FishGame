using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;


public class ScrollCircleAdvanced :ScrollRect, IPointerDownHandler, IPointerUpHandler
{
    //public SkillUIItem skillUiItem;
    public GameObject itemPressIcon;
    public GameObject itemPressIcon2;
    public Image itemPressBg;
    public GameObject itemStartPosition;
    public float mRadius = 0f;
    public float mEpsilon = 0f;
    private Vector2 _beginPos;  //可能会变
    private Vector2 _startPos = Vector2.zero;   //初始化之后就不会变
    private Vector2 _changePostion = Vector2.zero;
    private Vector2 _lastChangePostion = Vector2.zero;
    private Vector2 _defaultVector2 = Vector2.zero;
    private bool _isCancel;
    private bool _isDragging;
    private Action<Vector2> callbackAction; //每帧调用
    private Action<Vector2> backAction; //只有方向改变时调用
    private PointerEventData pointerEventData;
    private Vector3 _rotateAngle;

    protected override void Start()
    {
        base.Start();
        if (mRadius <= 0)
            mRadius = (transform as RectTransform).sizeDelta.x * 0.5f;
        if (mEpsilon <= 0)
            mEpsilon = content.sizeDelta.x * 0.5f;
        _beginPos = content.anchoredPosition;
        if (itemStartPosition != null)
            _startPos = itemStartPosition.GetRectTransform().anchoredPosition;
        _defaultVector2 = new Vector2(mRadius*2, mRadius*2);
    }

    public void InitDellAction(Action<Vector2> actin)
    {
        backAction = actin;
    }

    public void InitCallbackAction(Action<Vector2> action)
    {
        callbackAction = action;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (itemStartPosition != null)
        {
            Vector2 localPoint;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(this.viewRect, eventData.position, eventData.pressEventCamera, out localPoint))
                return;
            Vector2 dir = localPoint - _startPos;
            if (dir.sqrMagnitude > mRadius*mRadius)
            {
                _beginPos = localPoint - dir.normalized * mRadius;
                itemStartPosition.GetRectTransform().anchoredPosition = _beginPos;
                SetContentAnchoredPosition(localPoint);
                OnBeginDrag(eventData);
                OnDrag(eventData);
                pointerEventData = eventData;
            }
        }
        else
        {
//            PassEvent(eventData, ExecuteEvents.pointerDownHandler);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (itemStartPosition != null)
        {
            if (pointerEventData != null)
            {
                OnEndDrag(pointerEventData);
                pointerEventData = null;
            }
        }
        else
        {
//            PassEvent(eventData, ExecuteEvents.pointerUpHandler);
        }
    }

    public void PassEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function) where T : IEventSystemHandler
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);
        GameObject current = data.pointerCurrentRaycast.gameObject;
        for (int i = 0; i < results.Count; i++)
        {
            if (current != results[i].gameObject)
            {
                ExecuteEvents.Execute(results[i].gameObject, data, function);
                break;  //RaycastAll后ugui会自己排序，如果你只想响应透下去的最近一个响应，这里ExecuteEvents.Execute后直接break就行
            }
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        pointerEventData = null;
        _isDragging = true;
        _lastChangePostion = _defaultVector2;
        content.gameObject.SetActive(true);
        if (itemPressBg != null)
            itemPressBg.gameObject.SetActive(true);
        if (itemPressIcon != null)
            itemPressIcon.SetActive(false);
        if (itemPressIcon2 != null)
            itemPressIcon2.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        _isDragging = false;
        content.gameObject.SetActive(false);
        if (itemPressBg != null)
            itemPressBg.gameObject.SetActive(false);
        if (itemPressIcon != null)
            itemPressIcon.SetActive(true);
        if (itemPressIcon2 != null)
            itemPressIcon2.transform.localScale = Vector3.one;
        if (itemStartPosition != null)
        {
            itemStartPosition.GetRectTransform().anchoredPosition = _startPos;
            _beginPos = _startPos;
        }
        SetContentAnchoredPosition(_beginPos);
    }

    public override void OnDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        Vector2 localPoint;
        if (eventData.button != PointerEventData.InputButton.Left || !this.IsActive() || !RectTransformUtility.ScreenPointToLocalPointInRectangle(this.viewRect, eventData.position, eventData.pressEventCamera, out localPoint))
            return;

        var pointerPos = localPoint;

        Vector2 offsetPos = pointerPos - _beginPos;
        _changePostion = offsetPos.normalized;

        if (offsetPos.magnitude > mRadius)
        {
            offsetPos = _changePostion * mRadius;
            pointerPos = _beginPos + offsetPos;
        }
        SetContentAnchoredPosition(pointerPos);
        

        float angle = Vector2.Angle(Vector2.up, _changePostion);
        if (_changePostion.x > 0)
            angle = -angle;
        _rotateAngle.z = angle;
        content.localEulerAngles = _rotateAngle;
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
        
        Vector2 offsetPos = content.anchoredPosition - _beginPos;
        float distance = offsetPos.magnitude;
        if (distance <= mEpsilon)
        {
            _changePostion = Vector2.zero;
        }

        if (callbackAction != null)
            callbackAction(_changePostion);

        if (_changePostion != _lastChangePostion)
        {
            if (_isDragging && backAction != null)
            {
                backAction(_changePostion);
            }
            _lastChangePostion = _changePostion;
        }
    }

    public bool IsDragging { get { return _isDragging; } }
}
