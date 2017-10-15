using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;


public class ScrollCircle :ScrollRect
{
   // public SkillUIItem skillUiItem;
    public GameObject itemPressIcon;
    public Image itemPressBg;
    public GameObject itemStartPosition;
    public Image itemCancel;
    public float mRadius = 0f;
    public float mEpsilon = 0f;
    public bool IsUseEpsilon = false;
    private Vector2 _beginPos;  //可能会变
    private Vector2 _startPos = Vector2.zero;   //初始化之后就不会变
    private Vector2 _changePostion = Vector2.zero;
    private Vector2 _lastChangePostion = Vector2.zero;
    private Vector2 _lastRightStickPosition = Vector2.zero;
    private Vector2 _defaultVector2 = Vector2.zero;
    private bool _isCancel;
    private bool _isDragging;
    private Action<Vector2> callbackAction; //每帧调用
    private Action<Vector2> backAction; //只有方向改变时调用
    private Action<Vector2> successAction;  //成功施法时调用
    private PointerEventData pointerEventData;

    protected override void Start()
    {
        base.Start();
        if (mRadius <= 0)
            mRadius = (transform as RectTransform).sizeDelta.x * 0.5f;
        if (mEpsilon <= 0)
            mEpsilon = content.sizeDelta.x * 0.5f;
        _beginPos = content.anchoredPosition;
        if (itemStartPosition != null)
        {
            _startPos = itemStartPosition.GetRectTransform().anchoredPosition;
            _beginPos = _startPos;
        }
        _defaultVector2 = new Vector2(mRadius*2, mRadius*2);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        OnEndDrag(null);
    }
    
    public void InitDellAction(Action<Vector2> actin)
    {
        backAction = actin;
    }

    public void InitCallbackAction(Action<Vector2> action)
    {
        callbackAction = action;
    }

    public void InitSuccessAction(Action<Vector2> action)
    {
        successAction = action;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData != null)
        {
            OnBeginDrag(null);
            pointerEventData = eventData;   
        }
        else if(!_isDragging)//手柄操作触发
        {
            PointerEventData data = new PointerEventData(EventSystem.current);
            OnBeginDrag(data);
            pointerEventData = data;
        }
       // EventManager.Send<int, ConfigSerDataSkill>(EventIdx.OnBeginDrag, 0, skillUiItem.GetConfig());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (pointerEventData != null)
        {
            OnEndDrag(eventData);
            pointerEventData = null;
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData != null)
            base.OnBeginDrag(eventData);
        pointerEventData = null;
        _isDragging = true;
        _lastChangePostion = Vector2.zero;
        _lastRightStickPosition = Vector2.zero;
        _changePostion = Vector2.zero;
        if (content!=null)
            content.gameObject.SetActive(true);
        if (itemPressBg != null)
            itemPressBg.gameObject.SetActive(true);
        if (itemPressIcon != null && eventData != null)
            itemPressIcon.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        if (itemCancel != null)
        {
            itemCancel.gameObject.SetActive(true);
            _isCancel = false;
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (eventData != null)
            base.OnEndDrag(eventData);
        _isDragging = false;
        if (content!=null)
            content.gameObject.SetActive(false);
        if (itemPressBg != null)
            itemPressBg.gameObject.SetActive(false);
        if (itemPressIcon != null)
            itemPressIcon.transform.localScale = Vector3.one;
        if (content != null)
            SetContentAnchoredPosition(_beginPos);
        if (itemCancel != null)
        {
            itemCancel.gameObject.SetActive(false);
            if (!_isCancel && eventData != null)
            {
                if (successAction != null)
                    successAction(_changePostion);
            }
            else
            {
//                EventManager.Send(EventIdx.OnSkillCancel, false);
//                if (itemPressBg != null)
//                    itemPressBg.SetSpriteFight("bg_skill_enlarge_far2");
//                itemCancel.SetSpriteFight("icon_cancel_skill");
            }
        }
        //EventManager.Send<int>(EventIdx.OnEndDrag, 0);
    }
    
    public override void OnDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        Vector2 localPoint;
        if (eventData.button != PointerEventData.InputButton.Left || !this.IsActive() || !RectTransformUtility.ScreenPointToLocalPointInRectangle(this.viewRect, eventData.position, eventData.pressEventCamera, out localPoint))
            return;

        bool isCanceled = false;
        var pointerPos = localPoint;
        Vector2 offsetPos = pointerPos - _beginPos;
        _changePostion = offsetPos.normalized;
        float distance = offsetPos.magnitude;
        if (IsUseEpsilon && distance <= mEpsilon)
        {
            _changePostion = Vector2.zero;
        }
        else if (distance > mRadius*3)
        {
            offsetPos = _changePostion * mRadius;
            pointerPos = _beginPos + offsetPos;
            isCanceled = true;
        }
        else if (distance > mRadius)
        {
            offsetPos = _changePostion*mRadius;
            pointerPos = _beginPos + offsetPos;
        }
        else
        {
            _changePostion *= distance / mRadius;
        }
        SetContentAnchoredPosition(pointerPos);

        if (itemCancel != null)
        {
            if (isCanceled)
            {
                if (!_isCancel)
                {
                    _isCancel = true;
//                    EventManager.Send(EventIdx.OnSkillCancel, true);
//                    if (itemPressBg != null)
//                        itemPressBg.SetSpriteFight("bg_skill_enlarge_far_red2");
//                    itemCancel.SetSpriteFight("icon_cancel_skill_press");
                }
            }
            else
            {
                if (_isCancel)
                {
                    _isCancel = false;
//                    EventManager.Send(EventIdx.OnSkillCancel, false);
//                    if (itemPressBg != null)
//                        itemPressBg.SetSpriteFight("bg_skill_enlarge_far2");
//                    itemCancel.SetSpriteFight("icon_cancel_skill");
                }
            }
        } 
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

        //手柄操作放技能时候，按手柄摇杆模拟拖动技能按钮行为，改变指示器
//        if (_isDragging && skillUiItem.IsHandShanking)
//        {
//            if (!this.IsActive() || !JoystickView.Exists)
//                return;
//            if (_lastRightStickPosition == JoystickView.Instance.HankShankRightStick)
//                return;
//
//            _lastRightStickPosition = JoystickView.Instance.HankShankRightStick;
//            var pointerPos = _beginPos + _lastRightStickPosition * mRadius;
//            _changePostion = _lastRightStickPosition;
//            SetContentAnchoredPosition(pointerPos);
//        }

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
