using System;
using UnityEngine;

public class AccountManager : Singleton<AccountManager>
{
    public double Gold      { get; private set; }
    public int    BestStage { get; private set; } = 1010101;

    public event Action<double> OnGoldChanged;

    public void AddGold(double amount)
    {
        Gold += amount;

        OnGoldChanged?.Invoke(Gold);
    }

    public void UpdateBestStage(int currentStage)
    {
        TryUnlockNextStage(currentStage);
    }

    public void SetBestStage(int currentStage)
    {
        BestStage = currentStage;
    }

    private void TryUnlockNextStage(int clearID)
    {
        if (clearID == BestStage)
        {
            BestStage = GetNextStageID(clearID);
        }
    }

    private int GetNextStageID(int currentID)
    {
        int chapter = (currentID / 100) % 100;
        int stage   = currentID % 100;

        if (stage < 10)
            return currentID + 1;
        else
            return currentID + 100 - 9; // 다음 챕터로 이동 (1010110 -> 1010201)
    }
}