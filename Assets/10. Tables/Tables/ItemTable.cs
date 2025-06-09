using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemTable", menuName = "Tables/ItemTable", order = 0)]
public class ItemTable : BaseTable<int, ItemSO>
{
    protected override string[] DataPath => new[] { "Assets/10. Tables/ScriptableObj/Item/Consumable", "Assets/10. Tables/ScriptableObj/Item/Equipment" };

    public override void CreateTable()
    {
        Type = GetType();
        foreach (ItemSO item in dataList)
        {
            DataDic[item.ID] = item;
        }
    }
}