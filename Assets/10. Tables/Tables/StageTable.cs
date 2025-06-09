using System.Collections;
using System.Collections.Generic;
using _10._Tables.ScriptableObj;
using UnityEngine;

[CreateAssetMenu(fileName = "StageTable", menuName = "Tables/StageTable", order = 0)]
public class StageTable : BaseTable<int, StageSO>
{
    public override void CreateTable()
    {
        base.CreateTable();
        foreach (StageSO item in dataList)
        {
            DataDic[item.ID] = item;
        }
    }
}