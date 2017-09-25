using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class UICenterOnChild : MonoBehaviour, IEndDragHandler, IDragHandler
{
    public delegate void OnCenterHandler(BaseItemRender render, bool isCenter);
    public event OnCenterHandler OnCenterBegin;
    public event OnCenterHandler OnCenterEnd;
    private GridLayoutGroup _gridLayoutGroup;
    private RectTransform _contentRect;
    private ScrollRect _scrollView;
    private bool _isCentering;
    private Vector2 _targetPos;
    private float centerSpeed = 10;
    private BaseItemRender _curTarget;
    private Vector3 _maxScale = new Vector3(1.25f, 1.25f, 1f);
    private Vector3 _minScale = Vector3.one;

    public bool IsSensitive = false;    //开启敏感类型后，滑动（SensitiveParam个页面）就会滑到下一页
    public float SensitiveParam = 0.2f;

    void Awake()
    {
        _scrollView = gameObject.GetComponent<ScrollRect>();
        _scrollView.movementType = ScrollRect.MovementType.Unrestricted;
        _scrollView.inertia = false;
        _gridLayoutGroup = gameObject.GetComponentInChildren<GridLayoutGroup>();
        _contentRect = _scrollView.content;
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (_isCentering)
	    {
            _contentRect.anchoredPosition = Vector2.Lerp(_contentRect.anchoredPosition, _targetPos, centerSpeed * Time.deltaTime);
            if (Vector2.Distance(_contentRect.anchoredPosition, _targetPos) < Vector2.kEpsilon)
	        {
	            _isCentering = false;
	            if (_curTarget != null && OnCenterEnd != null)
	                OnCenterEnd(_curTarget, true);
	        }
	    }
        int axis = 0;
        int flag = 1;
        float gridLeftOrTopToAnchor = CalculateCurPos(ref axis, ref flag);
        for (int i = 0; i < _contentRect.childCount; i++)
        {
            RectTransform t = _contentRect.GetChild(i) as RectTransform;
            int g = flag > 0 ? 1 : 0;
            float curPos = gridLeftOrTopToAnchor;
            curPos += (axis - g) * _contentRect.sizeDelta[axis];
            /*
            if (axis == 1)
            {
                if (flag < 0)
                    curPos += _contentRect.sizeDelta[axis];
            }
            else if (axis == 0)
            {
                if (flag > 0)
                    curPos += -_contentRect.sizeDelta[axis];
            }*/
            curPos += t.anchoredPosition[axis];
            curPos += (0.5f - t.pivot[axis]) * t.sizeDelta[axis];
            t.localScale = Vector3.Lerp(_maxScale, _minScale, Mathf.Abs(curPos) / (_gridLayoutGroup.cellSize[axis] + _gridLayoutGroup.spacing[axis]));
        }
    }

    public void SetScale(Vector3 max, Vector3 min)
    {
        _maxScale = max;
        _minScale = min;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _isCentering = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isCentering = FindClosestPos(null);
    }
    
    public void CenterOn(BaseItemRender render)
    {
        _isCentering = FindClosestPos(render);
    }

    private bool FindClosestPos(BaseItemRender render)
    {
        bool canMove = false;
        if (_scrollView.vertical || _scrollView.horizontal)
        {
            int axis = 0;
            int flag = 1;
            float curPos = CalculateCurPos(ref axis, ref flag);
            if (_scrollView.vertical)
            {
                if (flag > 0)
                    curPos -= _gridLayoutGroup.padding.top;
                else
                    curPos += _gridLayoutGroup.padding.bottom;
            }
            else
            {
                if (flag > 0)
                    curPos -= _gridLayoutGroup.padding.right;
                else
                    curPos += _gridLayoutGroup.padding.left;
            }
            curPos -= _gridLayoutGroup.cellSize[axis] * 0.5f * flag;
            int idx;
            if (render != null)
            {
                idx = render.GetItemIndex();
            }
            else
            {
                idx = Mathf.RoundToInt(flag*curPos/(_gridLayoutGroup.cellSize[axis] + _gridLayoutGroup.spacing[axis]));
                if (IsSensitive && _curTarget != null && idx == _curTarget.GetItemIndex())
                {
                    float curTargetPos = _curTarget.GetItemIndex() * (_gridLayoutGroup.cellSize[axis] + _gridLayoutGroup.spacing[axis]);
                    if (flag * curPos - curTargetPos > _gridLayoutGroup.cellSize[axis] * SensitiveParam)
                        idx++;
                    else if (curTargetPos - flag * curPos > _gridLayoutGroup.cellSize[axis] * SensitiveParam)
                        idx--;
                }

                if (idx < 0)
                    idx = 0;
                else if (idx >= _contentRect.childCount)
                    idx = _contentRect.childCount - 1;
                render = _contentRect.GetChild(idx).GetComponent<BaseItemRender>();
            }
            float delta = flag * (_gridLayoutGroup.cellSize[axis] + _gridLayoutGroup.spacing[axis]) * idx - curPos;
            _targetPos = _contentRect.anchoredPosition;
            _targetPos[axis] += delta;

            canMove = true;
        }
        if (_curTarget != render)
        {
            if (_curTarget != null && OnCenterBegin != null)
                OnCenterBegin(_curTarget, false);
            _curTarget = render;
            if (OnCenterBegin != null)
                OnCenterBegin(_curTarget, true);
        }
        return canMove;
    }

    private float CalculateCurPos(ref int axis, ref int flag)
    {
        if (_scrollView.vertical)
        {
            axis = 1;
            if (_gridLayoutGroup.startCorner == GridLayoutGroup.Corner.UpperLeft || _gridLayoutGroup.startCorner == GridLayoutGroup.Corner.UpperRight)
                flag = 1;
            else
                flag = -1;
        }
        else
        {
            axis = 0;
            if (_gridLayoutGroup.startCorner == GridLayoutGroup.Corner.LowerLeft || _gridLayoutGroup.startCorner == GridLayoutGroup.Corner.UpperLeft)
                flag = -1;
            else
                flag = 1;
        }
        float curPos = _contentRect.anchoredPosition[axis];
        curPos += ((flag > 0 ? 1 : 0) - _contentRect.pivot[axis]) * _contentRect.sizeDelta[axis];
        RectTransform parentRect = _contentRect.parent as RectTransform;
        curPos += ((_contentRect.anchorMin[axis] + _contentRect.anchorMax[axis]) * 0.5f - 0.5f) * parentRect.sizeDelta[axis];

        return curPos;
    }

    public void Reset()
    {
        _curTarget = null;
    }
}
