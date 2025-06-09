using _10._Tables.ScriptableObj;
using UnityEngine;

namespace _10._Tables.Tables
{
    [CreateAssetMenu(fileName = "RewardTable", menuName = "Tables/RewardTable", order = 0)]
    public class RewardTable : BaseTable<string, RewardSO>
    {
        public override void CreateTable()
        {
            base.CreateTable();
            foreach (RewardSO item in dataList)
            {
                DataDic[item.ID] = item;
            }
        }
    }
}