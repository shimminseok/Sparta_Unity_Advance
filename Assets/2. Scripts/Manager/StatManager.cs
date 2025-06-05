using System;
using System.Collections;
using System.Collections.Generic;
using _10._Tables.ScriptableObj;
using UnityEngine;


public class StatManager : MonoBehaviour
{
    public Dictionary<StatType, StatBase> PlayerStat { get; private set; } = new Dictionary<StatType, StatBase>();


    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        var playerData = TableManager.Instance.GetTable<PlayerTable>();
        foreach (PlayerSO playerSo in playerData.dataDic.Values)
        {
            foreach (var stat in playerSo.PlayerStats)
            {
                PlayerStat[stat.StatType] = BaseStatFactory(stat.StatType, stat.Value);
            }
        }
    }

    private StatBase BaseStatFactory(StatType type, float value)
    {
        return type switch
        {
            StatType.CurHp => new ResourceStat(type, value),
            ///////////////////////////////////////////////////////////////////////////////////
            _ => new CalculatedStat(type, value),
        };
    }

    public T GetStat<T>(StatType type) where T : StatBase
    {
        return PlayerStat[type] as T;
    }

    public float GetValue(StatType type)
    {
        return PlayerStat[type].GetCurrent();
    }

    public void Recover(StatType statType, float value)
    {
        if (PlayerStat[statType] is ResourceStat res)
        {
            if (res.CurrentValue < res.MaxValue)
            {
                res.Recover(value);
                Debug.Log($"Recover : {statType} : {value} RemainValue: {res.CurrentValue}");
            }
        }
    }

    public void Consume(StatType statType, float value)
    {
        if (PlayerStat[statType] is ResourceStat res)
        {
            if (res.CurrentValue > 0)
            {
                res.Consume(value);
                Debug.Log($"Consume {statType} : {value}, RemainValue: {res.CurrentValue}");
            }
        }
    }

    public void ApplyStatEffect(StatType type, StatModifierType valueType, float value)
    {
        if (PlayerStat[type] is not CalculatedStat stat) return;

        switch (valueType)
        {
            case StatModifierType.Base:        stat.ModifyBaseValue(value); break;
            case StatModifierType.BuffFlat:    stat.ModifyBuffFlat(value); break;
            case StatModifierType.BuffPercent: stat.ModifyBuffPercent(value); break;
            case StatModifierType.Equipment:   stat.ModifyEquipmentValue(value); break;
        }

        switch (type)
        {
            case StatType.MaxHp:
                SyncCurrentWithMax(StatType.CurHp, stat);
                break;
        }

        Debug.Log($"Stat : {type} Modify Value {value}, FinalValue : {stat.Value}");
    }

    private void SyncCurrentWithMax(StatType curStatType, CalculatedStat stat)
    {
        if (PlayerStat.TryGetValue(curStatType, out var res) && res is ResourceStat curStat)
        {
            curStat.SetMax(stat.FinalValue);
        }
    }
}