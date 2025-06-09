using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageTable", menuName = "Tables/StageTable", order = 0)]
public class StageTable : BaseTable<int, StageSO>
{
    protected override string[] DataPath => new[] { "Assets/10. Tables/ScriptableObj/Stage" };

    public override void CreateTable()
    {
        Type = GetType();
        foreach (StageSO item in dataList)
        {
            DataDic[item.ID] = item;
        }
    }
}