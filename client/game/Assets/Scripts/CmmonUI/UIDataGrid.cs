using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIDataGrid : MonoBehaviour
{
    //注释要求：要求在panel加载前加载item资源  使用前要保证已经初始化
    public int IsNeedRestPos=0; //0表示置第一个，1表示不变，2表示最后一个，3表示置中点
    private GameObject RenderPrefab;
    private string _prefabPath;
    private System.Type _itemScript;
    private Direction _direction;
    private object[] _dataProvider;
    private Dictionary<int,BaseItemRender> _renders = new Dictionary<int,BaseItemRender>();
    List<BaseItemRender> items = new List<BaseItemRender>();
    private GameObject _content;
    private GridLayoutGroup _gridGroup;
    private Scrollbar _bar;
    private RectTransform _contTransform;
    private Vector2 _lastContentSize = Vector2.zero;
    private int _fullNum;
    private UIEventListener.VoidDelegate _clickHandler;
    private ScrollRect _scrollRect;
    private UICenterOnChild _uiCenterOnChild;
    
    private int _listMin;  //现在存在的list的最大最小值
    private int _listMax;
    private bool _isNeedSetMaxItemNum = false;
    private int _maxFirstInitNum = 0;
    private float _pivotPosition;
    private float _threshold;

    public float StartPosMin { get { return _startPosMin; } }
    private float _startPosMin;
    public float StartPosMax { get { return _startPosMax; } }
    private float _startPosMax;
    public GameObject ArrowImage1;
    public GameObject ArrowImage2;
    private bool _isFirstUpdate = false;
    public bool ResetPos = false;

    public int DataGridType = -1; //类型。用来区分使用同一类型Data的不同滚动列表


    /// <summary>
    /// 初始化datagrid信息
    /// </summary>
    /// <param name="itemPath">item的prefab位置</param>
    /// <param name="itemScriptName">item上的script的名字</param>
    /// <param name="startPostion">content</param>
    /// <param name="scrollRect">scrollrect,固定数量不可拖动不用去设置scroll</param>
    /// <param name="direction">设置方向 none设置不可拖动</param>
    /// <param name="isNeedSetMaxItemNum">使用wrapContent策略</param>
    public void InitDataGrid(string itemPath, string itemScriptName, GameObject content,ScrollRect scrollRect = null, Direction direction = Direction.Horizontal, bool isNeedSetMaxItemNum=false)
    {
        _itemScript = Type.GetType(itemScriptName);
        _prefabPath = itemPath;
        _direction = direction;
        _scrollRect = scrollRect;
        _content = content;
        _contTransform = _content.GetRectTransform();
        _isNeedSetMaxItemNum = isNeedSetMaxItemNum;
        _gridGroup=_content.GetComponent<GridLayoutGroup>();
        if (_scrollRect != null)
        {
            _uiCenterOnChild = _scrollRect.GetComponent<UICenterOnChild>();
            switch (direction)
            {
                case Direction.Horizontal:
                    _scrollRect.horizontal = true;
                    _scrollRect.vertical = false;
                    _bar = _scrollRect.horizontalScrollbar;
                    break;
                case Direction.Vertical:
                    _scrollRect.horizontal = false;
                    _scrollRect.vertical = true;
                    _bar = _scrollRect.verticalScrollbar;
                    break;
                case Direction.none:
                    _scrollRect.horizontal = false;
                    _scrollRect.vertical = false;
                    break;
            }
            RectTransform viewRect;
            if ((UnityEngine.Object)_scrollRect.viewport == (UnityEngine.Object) null)
                viewRect = _scrollRect.gameObject.GetRectTransform();
            else
                viewRect = _scrollRect.viewport;
            if (_gridGroup.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
            {
                int num = Mathf.FloorToInt((viewRect.rect.height - _gridGroup.padding.vertical + _gridGroup.spacing.y + 1.0f/1000.0f) / (_gridGroup.cellSize.y + _gridGroup.spacing.y));
                _fullNum = num * _gridGroup.constraintCount;
            }
            else if (_gridGroup.constraint == GridLayoutGroup.Constraint.FixedRowCount)
            {
                int num = Mathf.FloorToInt((viewRect.rect.width - _gridGroup.padding.horizontal + _gridGroup.spacing.x + 1.0f / 1000.0f) / (_gridGroup.cellSize.x + _gridGroup.spacing.x));
                _fullNum = num * _gridGroup.constraintCount;
            }
            else
            {
                int num1 = Mathf.FloorToInt((viewRect.rect.height - _gridGroup.padding.vertical + _gridGroup.spacing.y + 1.0f / 1000.0f) / (_gridGroup.cellSize.y + _gridGroup.spacing.y));
                int num2 = Mathf.FloorToInt((viewRect.rect.width - _gridGroup.padding.horizontal + _gridGroup.spacing.x + 1.0f / 1000.0f) / (_gridGroup.cellSize.x + _gridGroup.spacing.x));
                _fullNum = num1 * num2;
            }
        }
        else
        {
            _fullNum = -1;
        }
        
    }
    
    public object[] DataProvider
    {
        get { return _dataProvider; }
        set
        {
            _dataProvider = value;
            if (_dataProvider==null)
                _dataProvider = new object[0];
            
            UpdateUi();
            _isFirstUpdate = true;
            ResetPos = false;
        }
    }
    
    private void UpdateUi()
    {
        if (RenderPrefab == null)
            RenderPrefab = ResourceManager.Instance.LoadExistsAsset<GameObject>(_prefabPath);

        if (RenderPrefab == null)
            return;

        _gridGroup.enabled = true;

        int lastMaxFirstInitNum = _maxFirstInitNum;
        int lastListMin = _listMin;
        int lastListMax = _listMax;

        RectTransform viewRect = null;
        if (_scrollRect != null)
        {
            switch (_direction)
            {
                case Direction.Horizontal:
                    _scrollRect.horizontal = true;
                    _scrollRect.vertical = false;
                    break;
                case Direction.Vertical:
                    _scrollRect.horizontal = false;
                    _scrollRect.vertical = true;
                    break;
                case Direction.none:
                    _scrollRect.horizontal = false;
                    _scrollRect.vertical = false;
                    break;
            }
            if ((UnityEngine.Object)_scrollRect.viewport == (UnityEngine.Object)null)
                viewRect = _scrollRect.gameObject.GetRectTransform();
            else
                viewRect = _scrollRect.viewport;

            _scrollRect.onValueChanged.RemoveAllListeners();
            //设置需要创建的最大item数量
            if (_isNeedSetMaxItemNum)
            {
                if (_fullNum > 0)
                    _maxFirstInitNum = _fullNum + 4;

                if (_maxFirstInitNum > _dataProvider.Length)
                {
                    _maxFirstInitNum = 0;
                }
                else if (_bar != null)
                {
                    _scrollRect.horizontalScrollbar = null;
                    _scrollRect.verticalScrollbar = null;
                    _bar.gameObject.SetActive(false);
                }

                if (_maxFirstInitNum > 0)
                {
                    if (IsNeedRestPos == 0 || IsNeedRestPos == 3)
                    {
                        _listMin = 0;
                    }
                    else if (IsNeedRestPos == 1)
                    {
                        _listMin = lastListMin;
                        if (lastListMax > _dataProvider.Length - 1)
                            _listMin -= lastListMax - _dataProvider.Length + 1;
                        if (_listMin < 0)
                            _listMin = 0;
                    }
                    else if (IsNeedRestPos == 2)
                    {
                        _listMin = _dataProvider.Length - _maxFirstInitNum;
                    }
                    _listMax = _listMin + _maxFirstInitNum - 1;
                }
                else
                {
                    _listMin = 0;
                    _listMax = 0;
                }

                var oldPivot = _contTransform.pivot;
                int axis = _scrollRect.vertical ? 1 : 0;
                oldPivot[axis] = axis;
                _contTransform.pivot = oldPivot;

                _scrollRect.onValueChanged.AddListener(OnScroll);
            }
        }

        for (int i = 0; i < lastMaxFirstInitNum; i++)
        {
            _renders[lastListMin + i].gameObject.SetActive(false);
            _renders.Remove(lastListMin + i);
            if (_uiCenterOnChild != null)
            {
                Destroy(items[lastListMin + i].gameObject);
                items.RemoveAt(lastListMin + i);
            }
        }
        
        int initNum = _dataProvider.Length;
        if (_maxFirstInitNum != 0)
            initNum = _maxFirstInitNum;
        
        for (int i = 0; i < initNum; i++)
        {
            GameObject item;
            if (i >= _renders.Count)
            {
                if (i >= items.Count)
                {
                    item = UGUITools.AddChild(gameObject, RenderPrefab);
                    var itemRender = item.AddComponent(_itemScript) as BaseItemRender;
                    item.name = "item" + (i + _listMin);
                    _renders.Add(i + _listMin, itemRender);
                    items.Add(itemRender);
                }
                else
                {
                    items[i].gameObject.SetActive(true);
                    items[i].name = "item" + (i + _listMin);
                    _renders.Add(i + _listMin, items[i]);
                }
            }
            else    //如果_isNeedSetMaxItemNum为false，是不进入这里的，因为前面把_renders清掉了
            {
                item = _renders[i].gameObject;
                item.SetActive(true); //避免Render 里把自己给隐藏了
            }

            _renders[i + _listMin].SetItemIndex(i + _listMin, _dataProvider.Length);
            _renders[i + _listMin].SetDataGridType(DataGridType);
            _renders[i + _listMin].SetData(_dataProvider[i + _listMin]);

        }

        //如果_isNeedSetMaxItemNum为false，是不进入这里的，因为前面把_renders清掉了
        for (int i = _renders.Count - 1; i >= initNum; i--)
        {
            _renders[i].gameObject.SetActive(false);
            _renders.Remove(i);
            if (_uiCenterOnChild != null)
            {
                Destroy(items[i].gameObject);
                items.RemoveAt(i);
            }
        }

        if (_gridGroup is AutoSizeGridLayoutGroup)
        {
            (_gridGroup as AutoSizeGridLayoutGroup).ItemNum = _renders.Count;
        }

        if (_scrollRect != null)
        {
            //默认处理数量不足设置为不可拖动
            if (_renders.Count <= _fullNum && _uiCenterOnChild == null)
            {
                _scrollRect.horizontal = false;
                _scrollRect.vertical = false;
                if (_bar != null)
                {
                    _scrollRect.horizontalScrollbar = null;
                    _scrollRect.verticalScrollbar = null;
                    _bar.gameObject.SetActive(false);
                }
            }
            else
            {
                if (_direction == Direction.Horizontal)
                    _scrollRect.horizontalScrollbar = _bar;
                else if (_direction == Direction.Vertical)
                    _scrollRect.verticalScrollbar = _bar;
            }
            
            if (_direction == Direction.Horizontal)
            {
                int num;   //列数
                if (_gridGroup.constraint == GridLayoutGroup.Constraint.FixedRowCount)
                {
                    num = (_renders.Count + _gridGroup.constraintCount - 1) / _gridGroup.constraintCount;  //整数除法，向上取整
                }
                else if (_gridGroup.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
                {
                    num = _gridGroup.constraintCount;
                }
                else
                {
                    num = Mathf.FloorToInt((_contTransform.rect.height - _gridGroup.padding.vertical + _gridGroup.spacing.y + 1.0f / 1000.0f) / (_gridGroup.cellSize.y + _gridGroup.spacing.y));
                    num = Mathf.Max(1, num);
                    num = (_renders.Count + num - 1) / num;
                }
                _contTransform.sizeDelta = new Vector2(_gridGroup.padding.horizontal + num * (_gridGroup.cellSize.x + _gridGroup.spacing.x) - _gridGroup.spacing.x, _contTransform.sizeDelta.y);

                _gridGroup.CalculateLayoutInputHorizontal();
                _gridGroup.SetLayoutHorizontal();
                _gridGroup.CalculateLayoutInputVertical();
                _gridGroup.SetLayoutVertical();

                if (_listMax > 0)    //暂时不支持多行
                {
                    for (int i = 0; i < _renders.Count; i++)
                    {
                        var rectTran = _renders[i + _listMin].gameObject.GetRectTransform();
                        ResetPosition(rectTran, 1, _listMin * (_gridGroup.cellSize.x + _gridGroup.spacing.x));
                    }
                    _contTransform.sizeDelta = new Vector2(_gridGroup.padding.horizontal + (_listMax + 1) * (_gridGroup.cellSize.x + _gridGroup.spacing.x) - _gridGroup.spacing.x, _contTransform.sizeDelta.y);
                    _gridGroup.enabled = false;
                }

                float moveDistance = 0;
                float anchorGridToScrollview = (_contTransform.anchorMin.x + _contTransform.anchorMax.x) * 0.5f;
                _pivotPosition = (anchorGridToScrollview - 0.5f) * viewRect.rect.width;
                _threshold = (_gridGroup.cellSize.x + _gridGroup.spacing.x);
                if (IsNeedRestPos == 0)
                {
                    float dGridAnchToScrLeft = anchorGridToScrollview * viewRect.rect.width;
                    float dGridPivotToScrLeft = dGridAnchToScrLeft + _contTransform.anchoredPosition.x;
                    float dGridPivotToGridLeft = _contTransform.rect.width * _contTransform.pivot.x;
                    float dScrLeftToGridLeft = dGridPivotToGridLeft - dGridPivotToScrLeft;
                    moveDistance = dScrLeftToGridLeft;
                }
                else if (IsNeedRestPos == 1)
                {
                    if (_listMax > 0)
                    {
                        moveDistance = 0;
                    }
                    else if (_lastContentSize == Vector2.zero)
                    {
                        moveDistance = 0;
                    }
                    else
                    {
                        float delta = _contTransform.rect.width - _lastContentSize.x;
                        moveDistance = _contTransform.pivot.x * delta;
                    }
                }
                else if (IsNeedRestPos == 2)
                {
                    float dGridAnchToScrRight = (1 - anchorGridToScrollview) * viewRect.rect.width;
                    float dGridPivotToScrRight = dGridAnchToScrRight - _contTransform.anchoredPosition.x;
                    float dGridPivotToGridRight = _contTransform.rect.width * (1 - _contTransform.pivot.x);
                    float dScrRightToGridRight = dGridPivotToGridRight - dGridPivotToScrRight;
                    moveDistance = -dScrRightToGridRight;
                }
                else if (IsNeedRestPos == 3)
                {
                    float dGridPivotToScrCenter = _contTransform.anchoredPosition.x + (anchorGridToScrollview - 0.5f) * viewRect.rect.width;
                    float dGridPivotToGridCenter = _contTransform.rect.width * (_contTransform.pivot.x - 0.5f);
                    moveDistance = -dGridPivotToScrCenter + dGridPivotToGridCenter;
                }
                ResetPosition(_contTransform, 0, moveDistance);

                _startPosMin = (1 - anchorGridToScrollview) * viewRect.rect.width - _contTransform.rect.width * (1 - _contTransform.pivot.x);
                _startPosMax = _contTransform.rect.width * _contTransform.pivot.x - anchorGridToScrollview * viewRect.rect.width;
            }
            else if (_direction == Direction.Vertical)
            {
                float GroupLen = 0;
                if (_gridGroup is AutoSizeGridLayoutGroup)
                {
                    bool b = IsInScroll();
                    _gridGroup.CalculateLayoutInputHorizontal();
                    _gridGroup.SetLayoutHorizontal();
                    _gridGroup.CalculateLayoutInputVertical();
                    _gridGroup.SetLayoutVertical();

                    GroupLen = (_gridGroup as AutoSizeGridLayoutGroup).LayoutGroupHeight;
                    _contTransform.sizeDelta = new Vector2(_contTransform.sizeDelta.x, _gridGroup.padding.vertical + GroupLen);

                    if (ResetPos == false && b)
                    {
                        IsNeedRestPos = 1;
                    }
                    else
                    {
                        if (GroupLen + _gridGroup.padding.vertical >= viewRect.rect.height)
                            IsNeedRestPos = 2;
                        else
                        {
                            IsNeedRestPos = 0;
                        }
                    }
                }
                else
                {
                    int num;   //行数
                    if (_gridGroup.constraint == GridLayoutGroup.Constraint.FixedRowCount)
                    {
                        num = _gridGroup.constraintCount;
                    }
                    else if (_gridGroup.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
                    {
                        num = (_renders.Count + _gridGroup.constraintCount - 1) / _gridGroup.constraintCount;  //整数除法，向上取整
                    }
                    else
                    {
                        num = Mathf.FloorToInt((_contTransform.rect.width - _gridGroup.padding.horizontal + _gridGroup.spacing.x + 1.0f / 1000.0f) / (_gridGroup.cellSize.x + _gridGroup.spacing.x));
                        num = Mathf.Max(1, num);
                        num = (_renders.Count + num - 1) / num;
                    }
                    GroupLen = num * (_gridGroup.cellSize.y + _gridGroup.spacing.y) - _gridGroup.spacing.y;
                    _contTransform.sizeDelta = new Vector2(_contTransform.sizeDelta.x, _gridGroup.padding.vertical + GroupLen);

                    _gridGroup.CalculateLayoutInputHorizontal();
                    _gridGroup.SetLayoutHorizontal();
                    _gridGroup.CalculateLayoutInputVertical();
                    _gridGroup.SetLayoutVertical();
                }

                if (_listMax > 0)    //暂时不支持多列
                {
                    for (int i = 0; i < _renders.Count; i++)
                    {
                        var rectTran = _renders[i + _listMin].gameObject.GetRectTransform();
                        ResetPosition(rectTran, 1, -_listMin * (_gridGroup.cellSize.y + _gridGroup.spacing.y));
                    }
                    _contTransform.sizeDelta = new Vector2(_contTransform.sizeDelta.x, _gridGroup.padding.vertical + (_listMax + 1) * (_gridGroup.cellSize.y + _gridGroup.spacing.y) - _gridGroup.spacing.y);
                    _gridGroup.enabled = false;
                }
                
                float moveDistance = 0;
                float anchorGridToScrollview = (_contTransform.anchorMin.y + _contTransform.anchorMax.y) * 0.5f;
                _pivotPosition = (anchorGridToScrollview - 0.5f) * viewRect.rect.height;
                _threshold = (_gridGroup.cellSize.y + _gridGroup.spacing.y);
                if (IsNeedRestPos == 0)
                {
                    float dGridAnchToScrTop = (1 - anchorGridToScrollview) * viewRect.rect.height;
                    float dGridPivotToScrTop = dGridAnchToScrTop - _contTransform.anchoredPosition.y;
                    float dGridPivotToGridTop = _contTransform.rect.height * (1 - _contTransform.pivot.y);
                    float dScrTopToGridTop = dGridPivotToGridTop - dGridPivotToScrTop;
                    moveDistance = -dScrTopToGridTop;
                }
                else if (IsNeedRestPos == 1)
                {
                    if (_listMax > 0)
                    {
//                        float dGridTopToScrCenter = _pivotPosition + _contTransform.anchoredPosition.y;
//                        float dPointToGridTop = (((_listMax - _listMin + 1) >> 1) + _listMin) * _threshold;
//                        moveDistance = dPointToGridTop - dGridTopToScrCenter;
                    }
                    else if (_lastContentSize == Vector2.zero)
                    {
                        moveDistance = 0;
                    }
                    else
                    {
                        float delta = _contTransform.rect.height - _lastContentSize.y;
                        moveDistance = (_contTransform.pivot.y - 1) * delta;
                    }
                }
                else if (IsNeedRestPos == 2)
                {
                    float dGridAnchToScrBottom = anchorGridToScrollview * viewRect.rect.height;
                    float dGridPivotToScrBottom = dGridAnchToScrBottom + _contTransform.anchoredPosition.y;
                    float dGridPivotToGridBottom = _contTransform.rect.height * _contTransform.pivot.y;
                    float dScrBottomToGridBottom = dGridPivotToGridBottom - dGridPivotToScrBottom;
                    moveDistance = dScrBottomToGridBottom;
                }
                else if (IsNeedRestPos == 3)
                {
                    float dGridPivotToScrCenter = _contTransform.anchoredPosition.y + (anchorGridToScrollview - 0.5f) * viewRect.rect.height;
                    float dGridPivotToGridCenter = _contTransform.rect.height * (_contTransform.pivot.y - 0.5f);
                    moveDistance = -dGridPivotToScrCenter + dGridPivotToGridCenter;
                }

                ResetPosition(_contTransform, 1, moveDistance);

                _startPosMin = (1 - anchorGridToScrollview) * viewRect.rect.height - _contTransform.rect.height * (1 - _contTransform.pivot.y);
                _startPosMax = _contTransform.rect.height * _contTransform.pivot.y - anchorGridToScrollview * viewRect.rect.height;
            }
            _lastContentSize = _contTransform.sizeDelta;
        }
    }

    private static void ResetPosition(RectTransform tran, int axis, float moveDistance)
    {
        Vector2 v = tran.anchoredPosition;
        v[axis] += moveDistance;
        tran.anchoredPosition = v;
    }
    
    public bool IsInScroll()
    {
        if (_isFirstUpdate && _scrollRect != null && (_scrollRect.horizontal || _scrollRect.vertical))
        {
            if (_scrollRect.horizontal)
                return _scrollRect.horizontalNormalizedPosition >= 0.02f; //相对Right

            return _scrollRect.verticalNormalizedPosition >= 0.02f; //相对bottom
        }
        return false;
    }

    private void LateUpdate()
    {
        if (ArrowImage1 != null)
        {
            if (_scrollRect != null && (_scrollRect.horizontal || _scrollRect.vertical))
            {
                int axis = _scrollRect.horizontal ? 0 : 1;
                ArrowImage1.SetActive(_contTransform.anchoredPosition[axis] < _startPosMax);
                ArrowImage2.SetActive(_contTransform.anchoredPosition[axis] > _startPosMin);
            }
            else
            {
                ArrowImage1.SetActive(false);
                ArrowImage2.SetActive(false);
            }
        }
    }

    private void OnScroll(Vector2 offset)
    {
        if (_isNeedSetMaxItemNum && _maxFirstInitNum > 0)   //只有_scrollRect不为空的情况下_maxFirstInitNum才大于0，所以不判断_scrollRect != null
        {
            if (_scrollRect.horizontal)
            {
                float dGridPivotToScrCenter = _pivotPosition + _contTransform.anchoredPosition.x;
                float dGridPivotToGridTop = _contTransform.rect.width * _contTransform.pivot.x;
                float dGridTopToScrCenter = dGridPivotToScrCenter - dGridPivotToGridTop;
                float dPointToGridTop = (((_listMax - _listMin + 1) >> 1) + _listMin) * _threshold;
                float dPointToScrCenter = dGridTopToScrCenter + dPointToGridTop;
                if (_listMax < _dataProvider.Length - 1 && dPointToScrCenter < -_threshold)
                {
                    MoveTopToBottom(0);
                }
                else if (_listMin > 0 && dPointToScrCenter > _threshold)
                {
                    MoveBottomToTop(0);
                }
            }
            else if (_scrollRect.vertical)
            {
                float dGridPivotToScrCenter = _pivotPosition + _contTransform.anchoredPosition.y;
                float dGridPivotToGridTop = _contTransform.rect.height * (1 - _contTransform.pivot.y);
                float dGridTopToScrCenter = dGridPivotToScrCenter + dGridPivotToGridTop;
                float dPointToGridTop = (((_listMax - _listMin + 1) >> 1) + _listMin) * _threshold;
                float dPointToScrCenter = dGridTopToScrCenter - dPointToGridTop;
                if (_listMax < _dataProvider.Length - 1 && dPointToScrCenter > _threshold)
                {
                    MoveTopToBottom(1);
                }
                else if (_listMin > 0 && dPointToScrCenter < -_threshold)
                {
                    MoveBottomToTop(1);
                }
            }
        }
    }

    private void MoveTopToBottom(int axis)
    {
        int changenum = 1;
        if (_dataProvider.Length - 1 - _listMax < changenum)
            changenum = _dataProvider.Length - 1 - _listMax;
        for (int i = 0; i < changenum; i++)
        {
            BaseItemRender render = _renders[_listMin + i];
            render.SetItemIndex(_listMax + 1 + i, _dataProvider.Length);
            render.SetData(_dataProvider[_listMax + 1 + i]);
            render.gameObject.name = "item" + (_listMax + 1 + i);
            _renders.Remove(_listMin + i);
            _renders.Add(_listMax + 1 + i, render);
            var rectTran = render.gameObject.GetRectTransform();
//            rectTran.SetAsLastSibling();
            ResetPosition(rectTran, axis, (_listMax + 1 - _listMin) * (_gridGroup.cellSize[axis] + _gridGroup.spacing[axis]) * (axis == 1 ? -1 : 1));
            
            var oldSize = _contTransform.sizeDelta;
            float delta = changenum * (_gridGroup.cellSize[axis] + _gridGroup.spacing[axis]);
            oldSize[axis] += delta;
            _contTransform.sizeDelta = oldSize;

//            float moveDistance = _contTransform.pivot[axis] * delta * (axis == 1 ? -1 : 1);
//            ResetPosition(_contTransform, axis, moveDistance);
        }
        
        _listMin += changenum;
        _listMax += changenum;
    }

    private void MoveBottomToTop(int axis)
    {
        int changenum = 1;
        changenum = Math.Max(changenum, 1);
        if (_listMin < changenum)
            changenum = _listMin;
        for (int i = 0; i < changenum; i++)
        {
            BaseItemRender render = _renders[_listMax - i];
            render.SetItemIndex(_listMin - 1 - i, _dataProvider.Length);
            render.SetData(_dataProvider[_listMin - 1 - i]);
            render.gameObject.name = "item" + (_listMin - 1 - i);
            _renders.Remove(_listMax - i);
            _renders.Add(_listMin - 1 - i, render);
            var rectTran = render.gameObject.GetRectTransform();
//            rectTran.SetAsFirstSibling();
            ResetPosition(rectTran, axis, (_listMax + 1 - _listMin) * (_gridGroup.cellSize[axis] + _gridGroup.spacing[axis]) * (axis == 1 ? 1 : -1));

            var oldSize = _contTransform.sizeDelta;
            float delta = changenum * (_gridGroup.cellSize[axis] + _gridGroup.spacing[axis]);
            oldSize[axis] -= delta;
            _contTransform.sizeDelta = oldSize;

//            float moveDistance = _contTransform.pivot[axis] * delta * (axis == 1 ? 1 : -1);
//            ResetPosition(_contTransform, axis, moveDistance);
        }

        _listMin -= changenum;
        _listMax -= changenum;
    }


    public void Clear()
    {
        if (_renders != null)
        {
            foreach (var item in _renders)
            {
                item.Value.Clear();
            }
        }
    }


    public void AddClickEvent(UIEventListener.VoidDelegate action)
    {
        _clickHandler = action;
        foreach (var itemRender in _renders)
        {
            UGUIClickHandler.Get(itemRender.Value.gameObject).onPointerClick = OnPointerClick;
        }
    }

    public void OnPointerClick(GameObject go)
    {
        SetSelectItem(go.GetComponent<BaseItemRender>());
    }

    private void InternalMove(int index)
    {
        if (index < _listMin)
        {
            int num = _listMin - index + 2;
            if (num > _listMin)
                num = _listMin;
            for (int i = 0; i < num; i++)
                MoveBottomToTop(_scrollRect.horizontal ? 0 : 1);
        }
        else if (index > _listMax)
        {
            int num = index - _listMax + 2;
            if (num > _dataProvider.Length - 1 - _listMax)
                num = _dataProvider.Length - 1 - _listMax;
            for (int i = 0; i < num; i++)
                MoveTopToBottom(_scrollRect.horizontal ? 0 : 1);
        }
    }

    public void SetSelect(int index)
    {
        if (index < 0 || index > _dataProvider.Length - 1)
            return;
        if (_renders.ContainsKey(index))
            SetSelectItem(_renders[index]);
        else if (_listMax > 0)
        {
            if (currentSelect != null)
            {
                currentSelect.SetSelect(false);
                currentSelect = null;
            }
            InternalMove(index);
            SetSelectItem(_renders[index]);
        }
    }
    private BaseItemRender currentSelect;
    public void SetSelectItem(BaseItemRender render)
    {
        if(render!=null)
        {
            if (currentSelect != null)
                currentSelect.SetSelect(false);
            currentSelect = render;
            currentSelect.SetSelect(true);

            if (_clickHandler != null)
            {
                _clickHandler(render.gameObject);
            }

            render.SetSelect();

            if (_content != null && _scrollRect != null)
            {
                if (_uiCenterOnChild != null)
                {
                    _uiCenterOnChild.CenterOn(render);
                }
                else
                {
                    RectTransform viewRect;
                    if ((UnityEngine.Object)_scrollRect.viewport == (UnityEngine.Object)null)
                        viewRect = _scrollRect.gameObject.GetRectTransform();
                    else
                        viewRect = _scrollRect.viewport;
                    if (_scrollRect.vertical)
                    {
                        float anchorGridToScrollview = (_contTransform.anchorMin.y + _contTransform.anchorMax.y) * 0.5f;
                        float dGridAnchToScrTop = (1 - anchorGridToScrollview) * viewRect.rect.height;
                        float dGridPivotToScrTop = dGridAnchToScrTop - _contTransform.anchoredPosition.y;
                        float dGridPivotToGridTop = _contTransform.rect.height * (1 - _contTransform.pivot.y);
                        float dScrTopToGridTop = dGridPivotToGridTop - dGridPivotToScrTop;
                        RectTransform itemRectTransform = render.gameObject.GetRectTransform();
                        float dItemTopToGridTop = -itemRectTransform.anchoredPosition.y - (1 - itemRectTransform.pivot.y) * _gridGroup.cellSize.y;
                        if (dItemTopToGridTop < dScrTopToGridTop)   //上边缘被挡住了，需要下拉
                        {
                            float moveDistance = dItemTopToGridTop - dScrTopToGridTop;
                            Vector2 v = _contTransform.anchoredPosition;
                            v.y += moveDistance;
                            _contTransform.anchoredPosition = v;
                        }
                        else
                        {
                            float dGridAnchToScrBottom = anchorGridToScrollview * viewRect.rect.height;
                            float dGridPivotToScrBottom = dGridAnchToScrBottom + _contTransform.anchoredPosition.y;
                            float dGridPivotToGridBottom = _contTransform.rect.height * _contTransform.pivot.y;
                            float dScrBottomToGridBottom = dGridPivotToGridBottom - dGridPivotToScrBottom;
                            float dItemBottomToGridBottom = _contTransform.rect.height + itemRectTransform.anchoredPosition.y - itemRectTransform.pivot.y * _gridGroup.cellSize.y;
                            if (dItemBottomToGridBottom < dScrBottomToGridBottom) //下边缘被挡住了，需要上拉
                            {
                                float moveDistance = dScrBottomToGridBottom - dItemBottomToGridBottom;
                                Vector2 v = _contTransform.anchoredPosition;
                                v.y += moveDistance;
                                _contTransform.anchoredPosition = v;
                            }
                        }
                    }
                    else if (_scrollRect.horizontal)
                    {
                        float anchorGridToScrollview = (_contTransform.anchorMin.x + _contTransform.anchorMax.x) * 0.5f;
                        float dGridAnchToScrLeft = anchorGridToScrollview * viewRect.rect.width;
                        float dGridPivotToScrLeft = dGridAnchToScrLeft + _contTransform.anchoredPosition.x;
                        float dGridPivotToGridLeft = _contTransform.rect.width * _contTransform.pivot.x;
                        float dScrLeftToGridLeft = dGridPivotToGridLeft - dGridPivotToScrLeft;
                        RectTransform itemRectTransform = render.gameObject.GetRectTransform();
                        float dItemLeftToGridLeft = itemRectTransform.anchoredPosition.x - itemRectTransform.pivot.x * _gridGroup.cellSize.x;
                        if (dItemLeftToGridLeft < dScrLeftToGridLeft)   //左边缘被挡住了，需要右拉
                        {
                            float moveDistance = dScrLeftToGridLeft - dItemLeftToGridLeft;
                            Vector2 v = _contTransform.anchoredPosition;
                            v.x += moveDistance;
                            _contTransform.anchoredPosition = v;
                        }
                        else
                        {
                            float dGridAnchToScrRight = (1 - anchorGridToScrollview) * viewRect.rect.width;
                            float dGridPivotToScrRight = dGridAnchToScrRight - _contTransform.anchoredPosition.x;
                            float dGridPivotToGridRight = _contTransform.rect.width * (1 - _contTransform.pivot.x);
                            float dScrRightToGridRight = dGridPivotToGridRight - dGridPivotToScrRight;
                            float dItemRightToGridRight = _contTransform.rect.width - itemRectTransform.anchoredPosition.x + (1 - itemRectTransform.pivot.x) * _gridGroup.cellSize.x;
                            if (dItemRightToGridRight < dScrRightToGridRight) //右边缘被挡住了，需要左拉
                            {
                                float moveDistance = dItemRightToGridRight - dScrRightToGridRight;
                                Vector2 v = _contTransform.anchoredPosition;
                                v.x += moveDistance;
                                _contTransform.anchoredPosition = v;
                            }
                        }
                    }
                }
            }
        }
    }


    private bool isselect = false;
    public bool SetSelectItemBack<T>(string field, object value)
    {
        isselect = false;
        SetSelectItem<T>(field,value);
        return isselect;
    }

    public void SetSelectItem<T>(string field, object value)
    {
        if (!string.IsNullOrEmpty(field))
        {
            FieldInfo fi = typeof(T).GetField(field);
            if (fi != null)
            {
                for (int i = 0; i < _dataProvider.Length; i++)
                {
                    T itemData = (T)_dataProvider[i];
                    if (fi.GetValue(itemData).Equals(value))
                    {
                        if (_renders.ContainsKey(i))
                        {
                            SetSelectItem(_renders[i]);
                        }
                        else if (_listMax > 0)
                        {
                            if (currentSelect != null)
                            {
                                currentSelect.SetSelect(false);
                                currentSelect = null;
                            }
                            InternalMove(i);
                            SetSelectItem(_renders[i]);
                        }
                        isselect = true;
                        break;
                    }
                }
            }
        }
    }


    public Dictionary<int,BaseItemRender> GetRenders()
    {
        return _renders;
    }

    //重新赋值
    public void ReSetData(object[] dataP)
    {
        _dataProvider = dataP;
        foreach (KeyValuePair<int, BaseItemRender> kv in _renders)
        {
            if (kv.Key < _dataProvider.Length)
            {
                kv.Value.SetData(_dataProvider[kv.Key]);
            }
        }
    }

    public GameObject GetItem(int index)
    {
        if (_renders.ContainsKey(index))
            return _renders[index].gameObject;
        return null;
    }

    /// <summary>
    /// Get render by one field value of render's data
    /// </summary>
    /// <typeparam name="T">class of render's data</typeparam>
    /// <param name="field">field name</param>
    /// <param name="value">field value</param>
    /// <returns></returns>
    public BaseItemRender GetItem<T>(string field,object value)
    {
        if(!string.IsNullOrEmpty(field))
        {
            foreach (var render in _renders)
            {
                T itemData = (T)render.Value.Data;
                FieldInfo fi = typeof(T).GetField(field);
                if (fi != null)
                {
                    if (fi.GetValue(itemData).Equals(value))
                        return render.Value;
                }
                else
                {
                    PropertyInfo pi = typeof(T).GetProperty(field);
                    if (pi != null)
                    {
                        if (pi.GetValue(itemData, null).Equals(value))
                            return render.Value;
                    }
                    
                }
            }
        }
        return null;
    }


    public int GetItemIndex<T>(string field, object value)
    {
        if (!string.IsNullOrEmpty(field))
        {
            FieldInfo fi = typeof(T).GetField(field);
            if (fi!=null)
            {
                for (int i = 0; i < _dataProvider.Length; i++)
                {
                    T itemData = (T)_dataProvider[i];
                    if (fi.GetValue(itemData).Equals(value))
                        return i;
                }
            }
        }
        return 0;
    }

    /// <summary>
    /// 更新某项数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="field"></param>
    /// <param name="value"></param>
    /// <param name="newData"></param>
    public void UpdateItem<T>(string _field, object value, object newData, bool IsClickItem = true)
    {
        if (!string.IsNullOrEmpty(_field) && _renders != null)
        {
            foreach (var render in _renders)
            {
                string field = _field;
                T itemData = (T)render.Value.Data;
                string fontProperty = "";
                int num = field.IndexOf(".");
                if (num > -1)
                {
                    fontProperty = field.Substring(0, num);
                    field = field.Substring(num + 1, field.Length - 1-num);
                }
                Type type= typeof(T);
                FieldInfo fieldInfo_fa = null;
                if (!string.IsNullOrEmpty(fontProperty))
                {
                    fieldInfo_fa = typeof(T).GetField(fontProperty);
                    if (fieldInfo_fa != null)
                        type = fieldInfo_fa.FieldType;
                }
                FieldInfo fi = type.GetField(field);
                if (fi != null)
                {
                    if (fi.GetValue(fieldInfo_fa==null?itemData:fieldInfo_fa.GetValue(itemData)).Equals(value))
                    {
                        render.Value.SetData(newData);
                        if (IsClickItem && _clickHandler!=null)
                        {
                            _clickHandler(render.Value.gameObject);
                        }
                    }
                }
            }
        }
    }

}