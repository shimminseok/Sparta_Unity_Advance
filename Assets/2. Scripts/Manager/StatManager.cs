using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StatManager : MonoBehaviour
{
    public readonly Dictionary<StatType, StatBase> playerStat = new Dictionary<StatType, StatBase>();


    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        for (int i = 0; i < Enum.GetValues(typeof(StatType)).Length; i++)
        {
            playerStat[(StatType)i] = BaseStatFactory((StatType)i, 0);
        }
    }

    private StatBase BaseStatFactory(StatType type, float value)
    {
        return type switch
        {
            StatType.MaxHp          => new CalculatedStat(type, value),
            StatType.AttackPow      => new CalculatedStat(type, value),
            StatType.AttackSpd      => new CalculatedStat(type, value),
            StatType.MoveSpeed      => new CalculatedStat(type, 5),
            StatType.Defense        => new CalculatedStat(type, value),
            StatType.Dodge          => new CalculatedStat(type, value),
            StatType.CriticalChance => new CalculatedStat(type, value),
///////////////////////////////////////////////////////////////////////////////////
            StatType.CurHp => new ResourceStat(type, value),
            _              => null
        };
    }

    public T GetStat<T>(StatType type) where T : StatBase
    {
        return playerStat[type] as T;
    }

    public float GetValue(StatType type)
    {
        return playerStat[type].GetCurrent();
    }

    public void Recover(StatType statType, float value)
    {
        if (playerStat[statType] is ResourceStat res)
        {
            if (res.CurrentValue < res.MaxValue)
                res.Recover(value);
        }
    }

    public void Consume(StatType statType, float value)
    {
        if (playerStat[statType] is ResourceStat res)
        {
            if (res.CurrentValue > 0)
                res.Consume(value);
        }
    }

    public void ApplyStatEffect(StatType type, StatModifierType valueType, float value)
    {
        if (playerStat[type] is not CalculatedStat stat) return;

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
    }

    private void SyncCurrentWithMax(StatType curStatType, CalculatedStat stat)
    {
        if (playerStat.TryGetValue(curStatType, out var res) && res is ResourceStat curStat)
        {
            curStat.SetMax(stat.FinalValue);
        }
    }
}