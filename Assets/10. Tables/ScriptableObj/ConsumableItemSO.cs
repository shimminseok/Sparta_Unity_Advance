using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ConsumableItemSO", menuName = "ScriptableObject/Item/ConsumableItem", order = 0)]
public class ConsumableItemSO : ItemSO
{
    public List<StatusEffectData> StatusEffects;
    public float CoolTime;

    public bool IsStackable;

    [BoolShowIf("IsStackable")]
    public int MaxStack;
}