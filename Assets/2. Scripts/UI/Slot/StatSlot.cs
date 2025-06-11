using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI slotName;
    [SerializeField] private TextMeshProUGUI slotValue;

    private StatBase stat;

    public void SetSlot(StatBase stat)
    {
        slotName.text = stat.Type.ToString();
        this.stat = stat;
        UpdateSlot(stat.Value);
    }

    public void UpdateSlot(float value)
    {
        if (stat is ResourceStat resStat)
        {
            slotValue.text = $"{Utility.ToCurrencyString(resStat.Value): N0} / {Utility.ToCurrencyString(resStat.MaxValue): N0}";
        }
        else
        {
            slotValue.text = $"{Utility.ToCurrencyString(stat.Value): N1}";
        }
    }
}