using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class EquipmentItem : InventoryItem
{
    public bool IsEquipped;
    public int EnhanceLevel;

    public InventorySlot LinkedSlot;

    public new EquipmentItemSO ItemSo => base.ItemSo as EquipmentItemSO;

    public EquipmentItem(EquipmentItemSO itemSo, InventorySlot linkedSlot = null) : base(itemSo, 1)
    {
        IsEquipped = false;
        LinkedSlot = linkedSlot;
    }

    public EquipmentItem()
    {
    }

    public override InventoryItem Clone() => new EquipmentItem(ItemSo);

    public void Enhancement()
    {
        EnhanceLevel++;
        ItemChanged();
    }

    public string GetEnhanceCountStr()
    {
        return EnhanceLevel > 0 ? $"+{EnhanceLevel}" : string.Empty;
    }
}

public class EquipmentManager : MonoBehaviour
{
    public Dictionary<EquipmentType, EquipmentItem> EquipmentItems { get; private set; } = new Dictionary<EquipmentType, EquipmentItem>();

    public event Action<EquipmentType> OnEquipmentChanged;

    private GameManager gameManager => GameManager.Instance;


    public void EquipItem(EquipmentItem data)
    {
        EquipmentType type = data.ItemSo.EquipmentType;
        if (data.IsEquipped)
        {
            UnEquipItem(type);
            return;
        }

        if (EquipmentItems.ContainsKey(type))
        {
            UnEquipItem(type);
        }

        EquipmentItems[type] = data;
        foreach (StatData stat in data.ItemSo.Stats)
        {
            gameManager.PlayerController.StatManager.ApplyStatEffect(stat.StatType, StatModifierType.Equipment, stat.Value);
        }

        data.IsEquipped = true;
        data.LinkedSlot.SetEquipMark(true);
        Debug.Log($"아이템 장착 : {data.ItemSo.ItemName}");
        OnEquipmentChanged?.Invoke(type);
    }

    public void UnEquipItem(EquipmentType type)
    {
        if (!EquipmentItems.TryGetValue(type, out var item) || item == null)
            return;

        foreach (StatData stat in item.ItemSo.Stats)
        {
            gameManager.PlayerController.StatManager.ApplyStatEffect(stat.StatType, StatModifierType.Equipment, -stat.Value);
        }

        item.IsEquipped = false;
        item.LinkedSlot.SetEquipMark(false);
        EquipmentItems[type] = null;
        Debug.Log($"아이템 장착 해제 : {item.ItemSo.ItemName}");
        OnEquipmentChanged?.Invoke(type);
    }
}