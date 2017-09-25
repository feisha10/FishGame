using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class UIFlow : MonoBehaviour, IEndDragHandler, IDragHandler
{
    public delegate void LoadFunction(int page, int size);
    private event LoadFunction onLoad;

    private GridLayoutGroup _gridLayoutGroup;
    private RectTransform _contentRect;
    private ScrollRect _scrollView;
    private UIDataGrid _dataGrid;
    private int _curPage;
    private int _maxPage;
    private int _pageSize;
    
    public GameObject LoadingLast;
    public GameObject LoadingNext;

    void Awake()
    {
        _scrollView = gameObject.GetComponent<ScrollRect>();
        _contentRect = _scrollView.content;
        _gridLayoutGroup = gameObject.GetComponentInChildren<GridLayoutGroup>();
        _dataGrid = gameObject.GetComponentInChildren<UIDataGrid>();
    }

    public void Init(int pageSize, LoadFunction loadFunction, GameObject goLoadingNext = null, GameObject goLoadingLast = null)
    {
        _curPage = 1;
        _maxPage = int.MaxValue;
        _pageSize = pageSize;
        onLoad = loadFunction;
        LoadingNext = goLoadingNext;
        LoadingLast = goLoadingLast;
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (LoadingNext != null)
        {
            LoadingNext.SetActive(false);
        }
        if (LoadingLast != null)
        {
            LoadingLast.SetActive(false);
        }

        int axis = _scrollView.vertical ? 1 : 0;
        if (_contentRect.anchoredPosition[axis] > _dataGrid.StartPosMax && _curPage < _maxPage - 1)
        {
            //Next Page
            onLoad(_curPage + 1, _pageSize);
        }
        else if (_contentRect.anchoredPosition[axis] < _dataGrid.StartPosMin && _curPage > 1)
        {
            //Last Page
            onLoad(_curPage - 1, _pageSize);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        int axis = _scrollView.vertical ? 1 : 0;
        if (LoadingNext != null)
        {
            //Next Page
            if (_contentRect.anchoredPosition[axis] > _dataGrid.StartPosMax && _curPage < _maxPage - 1)
            {
                LoadingNext.SetActive(true);
            }
            else
            {
                LoadingNext.SetActive(false);
            }
        }
        if (LoadingLast != null)
        {
            //Last Page
            if (_contentRect.anchoredPosition[axis] < _dataGrid.StartPosMin && _curPage > 1)
            {
                LoadingLast.SetActive(true);
            }
            else
            {
                LoadingLast.SetActive(false);
            }
        }
    }
    
    public void DataBack(int pageIdx, int maxIdx)
    {
        if (LoadingNext != null)
        {
            LoadingNext.SetActive(false);
        }
        if (LoadingLast != null)
        {
            LoadingLast.SetActive(false);
        }

        int axis = _scrollView.vertical ? 1 : _scrollView.horizontal ? 0 : -1;
        if (axis > -1 && _curPage != pageIdx)
        {
            if (_curPage < pageIdx)
            {
                if (axis == 1)
                    TransformUtil.MoveY(_contentRect, -_pageSize * (_gridLayoutGroup.cellSize.y + _gridLayoutGroup.spacing.y));
                else
                    TransformUtil.MoveX(_contentRect, _pageSize * (_gridLayoutGroup.cellSize.x + _gridLayoutGroup.spacing.x));
            }
            else
            {
                if (axis == 1)
                    TransformUtil.MoveY(_contentRect, _pageSize * (_gridLayoutGroup.cellSize.y + _gridLayoutGroup.spacing.y));
                else
                    TransformUtil.MoveX(_contentRect, -_pageSize * (_gridLayoutGroup.cellSize.x + _gridLayoutGroup.spacing.x));
            }
        }
        
        _curPage = pageIdx;
        _maxPage = maxIdx;
    }
}