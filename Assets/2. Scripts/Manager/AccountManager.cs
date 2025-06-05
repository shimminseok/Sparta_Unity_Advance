using UnityEngine;

public class AccountManager : Singleton<AccountManager>
{
    public double Gold { get; private set; }


    public void AddGold(double amount)
    {
        Gold += amount;

        Debug.Log($"Gold : {Utility.ToCurrencyString(Gold)}");
    }
}