using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public delegate void CallbackTrigger();
public delegate void CallbackToggleTrigger(string selectToggleName, GameObject go);

/// <summary>
/// 管理Tab，下面挂的Tab必须是同一个group，Tab名称需不同.
/// SelectTab:选择标签 ;
/// SetTabEnable:设置标签是否显示 ;
/// GetTabItem:获取标签 ;
/// OnChange:标签页改变回调函数 ;
/// </summary>
[ExecuteInEditMode]
public class TabBar : MonoBehaviour
{
    public enum Arrangement
    {
        Horizontal,
        Vertical,
    }
    public CallbackToggleTrigger OnChange;//tab改变处理函数

    private Toggle[] _toggles;
    private ToggleGroup _gGroup;
    private Toggle _selectedItem;
    private int _defaultSize = 0;
    private int _selectSize = 0;
    private Color _defaultColor = Color.black;
    private Color _selectColor = Color.black;
    private bool _isSetColorAndSize = false;
    [HideInInspector]
    public bool IsReplaceTarget = false;//隐藏target图片

    //人工初始化
    public void ManulInit(Toggle[] toggles, ToggleGroup gGroup)
    {
        _toggles = toggles;
        _gGroup = gGroup;
        init();
    }

    private void init()
    {
        if (_selectedItem != null)
        {
            SetItemColorAndSize(_selectedItem, true);
            _selectedItem.isOn = false;
        }
        _selectedItem = null;
        if (_toggles != null) //动态初始化
        {
            for (int i = 0; i < _toggles.Length; i++)
            {
                _toggles[i].group = _gGroup;
                _toggles[i].onValueChanged.AddListener(SelectedChange);
            }
        }       
    }

    public void InitColorAndSize(Color defaultColor,Color selectColor,int defaultSize=0,int selectSize=0)
    {
        _defaultSize = defaultSize;
        _selectSize = selectSize;
        _defaultColor = defaultColor;
        _selectColor = selectColor;
        _isSetColorAndSize = true;
    }

    void SetItemColorAndSize(Toggle item, bool isdefault)
    {
        if (_isSetColorAndSize && item != null)
        {
            Color color = isdefault ? _defaultColor : _selectColor;
            int size = isdefault ? _defaultSize : _selectSize;
            
            Text txt = item.GetComponentInChildren<Text>();
            txt.color = color;
            if (size != 0)
            {
                txt.fontSize = size;
            }
        }
    }
    
    //检查有没有选中的tab  togglegroup中有
    private void SelectedChange(bool info)
    {
        var v = _gGroup.ActiveToggles().GetEnumerator();
        if(v.MoveNext())
        {
            Toggle item = v.Current;
            if (!_gGroup.allowSwitchOff && item != null && item != _selectedItem)
            {
                SetItemColorAndSize(_selectedItem,true);

                SetItemColorAndSize(item, false);

                if (_selectedItem!=null && IsReplaceTarget)
                {
                    _selectedItem.targetGraphic.gameObject.SetActive(true);
                }

                _selectedItem = item;
                if (OnChange != null)
                {
                    if (IsReplaceTarget)
                    {
                        _selectedItem.targetGraphic.gameObject.SetActive(false);
                    }
                    OnChange(_selectedItem.name, _selectedItem.gameObject);
                }
            }
        }
        
    }

    /// <summary>
    /// 根据index获取toggle组件
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Toggle GetTabItem(int index)
    {
        Toggle toggle = null;
        if (index >= 0 && index < _toggles.Length)
            toggle = _toggles[index];
        return toggle;
    }
    /// <summary>
    /// 根据组件名称获取toggle组件
    /// </summary>
    /// <param name="tabName"></param>
    /// <returns></returns>
    public Toggle GetTabItem(string tabName)
    {
        for (int i = 0; i < _toggles.Length; i++)
        {
            if (_toggles[i].name == tabName)
                return _toggles[i];
        }
        return null;
    }

    /// <summary>
    /// 根据tab index选择标签页
    /// </summary>
    /// <param name="index"></param>
    /// <param name="alwaysChange">即使选择了现在已经是isOn的toggle也调用OnChange</param>
    public void SelectTab(int index, bool alwaysChange = false)
    {
        Toggle toggle = GetTabItem(index);
        SelectTab(toggle, alwaysChange);  
    }

    /// <summary>
    /// 根据tab名称选择标签页
    /// </summary>
    /// <param name="tabName"></param>
    /// <param name="alwaysChange">即使选择了现在已经是isOn的toggle也调用OnChange</param>
    public void SelectTab(string tabName, bool alwaysChange = false)
    {
        Toggle toggle = GetTabItem(tabName);
        SelectTab(toggle, alwaysChange);
    }


    public void SelectTab(Toggle toggle, bool alwaysChange)
    {
        if (toggle == null)
        {
            return;
        }

        if (toggle.gameObject.activeSelf)
        {
            toggle.isOn = true;
            if (alwaysChange || _selectedItem != toggle)
            {
                SetItemColorAndSize(_selectedItem, true);
                _selectedItem = toggle;
                SetItemColorAndSize(_selectedItem, false);
                if (OnChange != null)
                {
                    OnChange(_selectedItem.name, _selectedItem.gameObject);
                }
            }
        }
        else
        {
            _gGroup.allowSwitchOff = true;
            foreach (var tab in _toggles)
            {
                tab.isOn = false;
            }
        }
    }

    public void SelectTabReset()
    {
        _gGroup.allowSwitchOff = true;
        foreach (var tab in _toggles)
        {
            tab.isOn = false;
        }
    }

    /// <summary>
    /// 根据index设置标签页是否显示
    /// </summary>
    /// <param name="index"></param>
    /// <param name="isVisiable"></param>
    public void SetTabVisiable(int index, bool isVisiable)
    {
        Toggle toggle = GetTabItem(index);
        SetTabVisiable(toggle, isVisiable);
    }

    /// <summary>
    /// 根据tab名称设置标签页是否显示
    /// </summary>
    /// <param name="tabName"></param>
    /// <param name="isVisiable"></param>
    public void SetTabVisiable(string tabName, bool isVisiable)
    {
        Toggle toggle = GetTabItem(tabName);
        SetTabVisiable(toggle, isVisiable);
    }

    public void SetTabVisiable(Toggle toggle,bool isVisiable)
    {
        if (toggle == null)
            return;
        if (isVisiable && !toggle.gameObject.activeSelf)
        {
            toggle.gameObject.SetActive(true);
        }
        else if (!isVisiable && toggle.gameObject.activeSelf)
        {
            toggle.gameObject.SetActive(false);
        }
    }

    public void SetTabEnable(string tabName, bool isEnable)
    {
        Toggle toggle = GetTabItem(tabName);
        if(toggle!=null)
            toggle.interactable = isEnable;
    }

    public void AddClickEvent(string tabName,UIEventListener.VoidDelegate clickHandler)
    {
        Toggle toggle = GetTabItem(tabName);
        if (toggle != null)
            UIEventListener.Get(toggle.gameObject).onClick = clickHandler;
    }

    public int selectIndex
    {
        get
        {
            for (int i = 0; i < _toggles.Length; i++)
            {
                if (_toggles[i].isOn)
                    return i;
            }
            return -1;
        }
        set
        {
            SelectTab(value);
        }
    }

    public Toggle selectedItem
    {
        get { return _selectedItem; }
    }

    public Toggle[] AllToggle { get { return _toggles; } }
}
