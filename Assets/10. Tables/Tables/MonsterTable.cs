using System.Collections;
using System.Collections.Generic;
using _10._Tables.ScriptableObj;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterTable", menuName = "Tables/MonsterTable", order = 0)]
public class MonsterTable : BaseTable<MonsterSO>
{
    public override void CreateTable()
    {
        base.CreateTable();
        foreach (MonsterSO item in dataList)
        {
            dataDic[item.ID] = item;
        }
    }
}