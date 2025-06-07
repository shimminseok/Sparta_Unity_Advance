using System;
using UnityEngine;

public class AccountManager : Singleton<AccountManager>
{
    public double Gold { get; private set; }


    public event Action<double> OnGoldChanged;

    public void AddGold(double amount)
    {
        Gold += amount;

        Debug.Log($"Gold : {Utility.ToCurrencyString(Gold)}");

        OnGoldChanged?.Invoke(Gold);
    }
}