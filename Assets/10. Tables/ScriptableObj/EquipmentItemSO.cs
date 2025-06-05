using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentItemSO", menuName = "ScriptableObject/Item/EquipItem", order = 0)]
public class EquipmentItemSO : ItemSO
{
    public EquipmentType EquipmentType;
    public List<StatData> Stats;
}