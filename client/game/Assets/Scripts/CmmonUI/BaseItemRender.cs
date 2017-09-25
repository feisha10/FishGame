using UnityEngine;

public class BaseItemRender : MonoBehaviour
{
	public object Data;

    protected int ItemIndex;
    protected int ItemNum; // 总个数
    protected int DataGridType = -1; //类型。用来区分使用同一类型Data的不同滚动列表

    public int ItemHeight;
    public int YOffset;

	public virtual void SetData(object data)
	{
		Data = data;
	}

    public virtual void SetSelect()
    {
    }

    public virtual void SetSelect(bool isSelect)
    {
    }

    public void SetItemIndex(int index,int num)
    {
        ItemIndex = index;
        ItemNum = num;
    }

    public void SetDataGridType(int dataGridType)
    {
        DataGridType = dataGridType;
    }

    public int GetItemIndex()
    {
        return ItemIndex ;
    }

    public T GetData<T>()
    {
        return (T)Data;
    }

    public void Refresh()
    {
        SetData(Data);
    }

    public virtual int GetItemSpan()
    {
        return 0;
    }

    public virtual void Clear()
    {        
    }
}
