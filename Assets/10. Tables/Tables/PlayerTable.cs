using System.Collections;
using System.Collections.Generic;
using _10._Tables.ScriptableObj;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerTable", menuName = "Tables/PlayerTable", order = 0)]
public class PlayerTable : BaseTable<PlayerSO>
{
    public override void CreateTable()
    {
        base.CreateTable();
        foreach (PlayerSO item in dataList)
        {
            dataDic[item.ID] = item;
        }
    }
}