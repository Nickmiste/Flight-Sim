using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInitializable { void Init(object[] data); }

public class DataString
{
    private string data;
    
    public DataString Add(string key, object value)
    {
        data += string.Format("{0}|{1}|{2},", value.GetType(), key, value);
        return this;
    }
    
    public T Get<T>(string key) where T : class
    {
        foreach (string str in data.Split(','))
            if (str.Split('|')[1].Equals(key))
                if (Type.GetType(str.Split('|')[0]) is T)
                    return str.Split('|')[2] as T;
        return null;
    }
}

public class DataStringBuilder
{
    private string str;
    public string GetString() => str;

    public DataStringBuilder Add(string name, object value)
    {
        str += string.Format("{0}|{1}|{2},", value.GetType(), name, value);
        return this;
    }
}

public static class Initializer
{
    //public static void Init(object obj, byte data) => (obj as Initializable8)?.Init(data);
    //public static void Init(object obj, ushort data) => (obj as Initializable16)?.Init(data);
    //public static void Init(object obj, uint data) => (obj as Initializable32)?.Init(data);
    //public static void Init(object obj, ulong data) => (obj as Initializable64)?.Init(data);
}

public static class DataUtil
{

}