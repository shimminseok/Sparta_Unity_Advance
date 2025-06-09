using System;
using System.Collections;
using System.Collections.Generic;
using _10._Tables.ScriptableObj;
using UnityEngine;


public class StatManager : MonoBehaviour
{
    public Dictionary<StatType, StatBase> Stats { get; private set; } = new Dictionary<StatType, StatBase>();


    private void Awake()
    {
    }

    /// <summary>
    /// 플레이어의 스탯을 초기화 시켜주는 메서드
    /// </summary>
    /// <param name="player"></param>
    public void Initialize(PlayerSO player)
    {
        foreach (var stat in player.PlayerStats)
        {
            Stats[stat.StatType] = BaseStatFactory(stat.StatType, stat.Value);
        }
    }

    /// <summary>
    /// 몬스터의 스탯을 초기화 시켜주는 메서드
    /// </summary>
    /// <param name="monster"></param>
    public void Initialize(MonsterSO monster)
    {
        foreach (StatData monsterStat in monster.Stats)
        {
            Stats[monsterStat.StatType] = BaseStatFactory(monsterStat.StatType, monsterStat.Value);
        }
    }

    private StatBase BaseStatFactory(StatType type, float value)
    {
        return type switch
        {
            StatType.CurHp => new ResourceStat(type, value),
            StatType.CurMp => new ResourceStat(type, value),
            ///////////////////////////////////////////////////////////////////////////////////
            _ => new CalculatedStat(type, value),
        };
    }

    public T GetStat<T>(StatType type) where T : StatBase
    {
        return Stats[type] as T;
    }

    public float GetValue(StatType type)
    {
        return Stats[type].GetCurrent();
    }

    public void Recover(StatType statType, float value)
    {
        if (Stats[statType] is ResourceStat res)
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
        if (Stats[statType] is ResourceStat res)
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
        if (Stats[type] is not CalculatedStat stat) return;

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
        if (Stats.TryGetValue(curStatType, out var res) && res is ResourceStat curStat)
        {
            curStat.SetMax(stat.FinalValue);
        }
    }
}