using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Consume,
    Eqiupment,
}

public enum StatType
{
    MaxHp,
    CurHp,

    AttackPow,
    AttackSpd,

    MoveSpeed,
    Defense,

    Dodge,
    CriticalChance,
}

public enum StatModifierType
{
    Base,
    BuffFlat,
    BuffPercent,
    Equipment
}