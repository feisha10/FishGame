using System;
using System.Collections.Generic;
using System.Reflection;

public class SortUtil
{
    private static string[] _args;
    private static int _index = 0;

    /// <summary>
    /// List排序，args为需要排序的属性（目前只支持int,long和bool类型,int枚举类型,string类型，默认从小到大，false比true优先，属性前面加!符号表示该字段反向排序）
    /// 支持public属性 与 public get set 属性
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static List<T> Sort<T>(List<T> list, string[] args)
    {
        if (list == null || list.Count == 0 || args == null || args.Length == 0) return list;
        _args = args;
        _index = 0;
        list.Sort(Sort);
        return list;
    }

    public static T[] Sort<T>(T[] array,string[] args)
    {
        if (array == null || array.Length == 0 || args == null || args.Length == 0) return array;
        _args = args;
        _index = 0;
        Array.Sort(array, Sort);
        return array;
//         List<T> list = new List<T>(array);
//         list = Sort(list, args);
//         return list.ToArray();
    }

    private static int Sort<T>(T a, T b)
    {
        if (_index == _args.Length) return 0;
        string s = _args[_index];
        int value = Sort(a, b, s);
        if (value == 0)
        {
            _index++;
            value = Sort(a, b);
        }
        _index = 0;
        return value;
    }

    private static int Sort<T>(T a, T b, string p)
    {
        if (a == null || b == null)
            return 0;

        bool isDesc = false;
        string property;
        string fontProperty="";
        if (p.IndexOf("!") > -1)
        {
            property = p.Substring(1, p.Length - 1);
            isDesc = true;
        }
        else
        {
            property = p;
        }
        int num = property.IndexOf(".");
        if (num > -1)
        {
            fontProperty = property.Substring(0,num);
            property = property.Substring(num+1, property.Length- num - 1);
        }
        Type type_a = a.GetType();
        FieldInfo fieldInfo_fa=null;
        if (!string.IsNullOrEmpty(fontProperty))
        {
            fieldInfo_fa = type_a.GetField(fontProperty);
            if (fieldInfo_fa != null)
                type_a=fieldInfo_fa.FieldType;
        }
            
        FieldInfo fieldInfo_a = type_a.GetField(property);//支持public字段
        object obj_a;
        if (fieldInfo_a == null)
        {
            PropertyInfo pInfor_a = type_a.GetProperty(property);// 支持get set 属性
            obj_a = pInfor_a.GetValue(fieldInfo_fa!=null? fieldInfo_fa.GetValue(a):a, null);
        }
        else
        {
            obj_a=fieldInfo_a.GetValue(fieldInfo_fa != null ? fieldInfo_fa.GetValue(a) : a);
        }
        
        Type type_b = b.GetType();
        FieldInfo fieldInfo_fb = null;
        if (!string.IsNullOrEmpty(fontProperty))
        {
            fieldInfo_fb = type_b.GetField(fontProperty);
            if (fieldInfo_fb != null)
                type_b = fieldInfo_fb.FieldType;
        }
        FieldInfo fieldInfo_b = type_b.GetField(property);
        object obj_b;
        if (fieldInfo_b==null)
        {
            PropertyInfo pInfor_b = type_b.GetProperty(property);// 支持get set 属性
            obj_b = pInfor_b.GetValue(fieldInfo_fb != null ? fieldInfo_fb.GetValue(b) : b, null);
        }
        else
        {
            obj_b = fieldInfo_b.GetValue(fieldInfo_fb != null ? fieldInfo_fb.GetValue(b) : b);
        }
        int value_a;
        if (obj_a is bool)
        {
            value_a = ((bool)obj_a) ? 1 : 0;
            int value_b = ((bool)obj_b) ? 1 : 0;
            return Sort(value_a, value_b, isDesc);
        }
        else if (obj_a is int)
        {
            value_a = (int)obj_a;
            int value_b = (int)obj_b;
            return Sort(value_a, value_b, isDesc);
        }
        else if(obj_a is Enum)
        {
            value_a = (int)obj_a;
            int value_b = (int)obj_b;
            return Sort(value_a, value_b, isDesc);
        }
        else if(obj_a is string)
        {
            return String.Compare((string) obj_a, (string) obj_b);
        }
        else if (obj_a is long)
        {
            long long_a = (long)obj_a;
            long long_b = (long)obj_b;
            return Sort(long_a, long_b, isDesc);
        }       
        else return 0;
    }

    private static int Sort(int a, int b, bool isDesc)
    {
        if (a < b)
        {
            if (isDesc) return 1;
            else return -1;
        }
        else if (a > b)
        {
            if (isDesc) return -1;
            else return 1;
        }
        return 0;
    }

    private static int Sort(long a, long b, bool isDesc)
    {
        if (a < b)
        {
            if (isDesc) return 1;
            else return -1;
        }
        else if (a > b)
        {
            if (isDesc) return -1;
            else return 1;
        }
        return 0;
    }
}