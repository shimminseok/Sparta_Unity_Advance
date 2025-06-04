using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemSO", menuName = "ScriptableObject/Item", order = 0)]
public class ItemSO : ScriptableObject
{
    public int ID;
    public string ItemName;
    public string ItemDescription;
    public ItemType ItemType;
    public Sprite ItemSprite;
    public bool IsStackable;
    public List<Stat> Stats;
    public int ItemGrade;

    [BoolShowIf("IsStackable")]
    public int MaxStack;
}