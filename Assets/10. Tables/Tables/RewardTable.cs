using UnityEngine;

[CreateAssetMenu(fileName = "RewardTable", menuName = "Tables/RewardTable", order = 0)]
public class RewardTable : BaseTable<string, RewardSO>
{
    protected override string[] DataPath => new[] { "Assets/10. Tables/ScriptableObj/Reward" };

    public override void CreateTable()
    {
        Type = GetType();
        foreach (RewardSO item in dataList)
        {
            DataDic[item.ID] = item;
        }
    }
}