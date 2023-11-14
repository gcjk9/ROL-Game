using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Tools { 
    public static byte[] ObjectToBytes(object obj)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            IFormatter formatter = new BinaryFormatter(); formatter.Serialize(ms, obj); return ms.GetBuffer();
        }
    }

    /// <summary> 
    /// 将一个序列化后的byte[]数组还原         
    /// </summary>
    /// <param name="Bytes"></param>         
    /// <returns></returns> 
    public static object BytesToObject(byte[] Bytes)
    {
        using (MemoryStream ms = new MemoryStream(Bytes))
        {
            IFormatter formatter = new BinaryFormatter(); return formatter.Deserialize(ms);
        }
    }

    public static string ByteTostring(byte[] bytes)
    {
        return System.Text.Encoding.Default.GetString(bytes);
    }
    public static byte[] StringTobyte(string str)
    {
        return System.Text.Encoding.Default.GetBytes(str);
    }
    public static T GetObjectByName<T>(T t, string propertyName) where T : new()
    {
        //T t = new T();
        System.Reflection.PropertyInfo[] propertys = t.GetType().GetProperties();
        foreach (System.Reflection.PropertyInfo p in propertys)
        {
            if (p.PropertyType == Type.GetType(propertyName))
            {
                t = (T)p.GetValue(t, null);
            }
        }
        return t;
    }
    public static T CreateInstance<T>() where T : new()
    {
        return new T();
    }
}
