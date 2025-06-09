using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemTable", menuName = "Tables/ItemTable", order = 0)]
public class ItemTable : BaseTable<int, ItemSO>
{
    public override void CreateTable()
    {
        base.CreateTable();
        foreach (ItemSO item in dataList)
        {
            DataDic[item.ID] = item;
        }
    }
}