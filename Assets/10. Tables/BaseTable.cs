using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BaseTable<TKey, TValue> : ScriptableObject, ITable where TKey : notnull
{
    [SerializeField] protected List<TValue> dataList = new List<TValue>();

    public Dictionary<TKey, TValue> DataDic { get; private set; } = new Dictionary<TKey, TValue>();

    public Type Type { get; private set; }

    public virtual void CreateTable()
    {
        Type = GetType();
    }

    public virtual TValue GetDataByID(TKey id)
    {
        if (DataDic.TryGetValue(id, out TValue value))
            return value;

        Debug.LogError($"ID {id}를 찾을 수 없습니다. Type :{Type})");
        return default;
    }
}