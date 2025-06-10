using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : SceneOnlySingleton<RewardManager>
{
    protected override void Awake()
    {
        base.Awake();
    }


    public void GetReward(RewardSO reward)
    {
        if (reward.HasRewardGold)
        {
            AccountManager.Instance.AddGold(reward.RewardGold);
        }

        if (reward.HasRewardExp)
        {
        }

        foreach (var rewardItem in reward.RewardItems)
        {
            InventoryItem item = new InventoryItem(rewardItem.Item, rewardItem.Quantity);
            InventoryManager.Instance.AddItem(item);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}