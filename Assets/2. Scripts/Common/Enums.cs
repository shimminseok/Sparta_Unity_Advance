using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Consume,
    Eqiupment,
}

public enum EquipmentType
{
    Waepon,
    Helmet,
    Aromor,
    Boots,
    Gloves
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

    MaxMp,
    CurMp,

    AttackRange,
}

public enum StatModifierType
{
    Base,
    BuffFlat,
    BuffPercent,
    Equipment
}

public enum Direction
{
    Forward,
    Backward,
    Left,
    Right
}