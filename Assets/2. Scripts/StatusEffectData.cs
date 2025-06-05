using System;
using UnityEngine;


public enum StatusEffectType
{
    InstantBuff,
    OverTimeBuff,
    InstantDebuff,
    OverTimeDebuff,
    TimedModifierBuff,
    PeriodicDamageDebuff,
    Recover,
}


[Serializable]
public class StatusEffectData
{
    public StatusEffectType EffectType;
    public StatData Stat;
    public float Duration;
    public float TickInterval;
}