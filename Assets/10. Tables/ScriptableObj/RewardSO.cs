using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RewardSO", menuName = "ScriptableObject/RewardSO", order = 0)]
public class RewardSO : ScriptableObject
{
    public string ID;
    public string RewardName;
    public bool HasRewardGold;

    [BoolShowIf("HasRewardGold")]
    public int RewardGold;

    public bool HasRewardExp;

    [BoolShowIf("HasRewardExp")]
    public int RewardExp;

    public List<ItemSO> RewardItems;
}