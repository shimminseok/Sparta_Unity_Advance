using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI slotName;
    [SerializeField] private TextMeshProUGUI slotValue;


    public void UpdateSlot(StatBase stat)
    {
        slotName.text = stat.Type.ToString();

        if (stat is ResourceStat resStat)
        {
            slotValue.text = $"{resStat.CurrentValue:N0} / {resStat.MaxValue:N0}";
        }
        else
        {
            slotValue.text = stat.Value.ToString("N0");
        }
    }
}