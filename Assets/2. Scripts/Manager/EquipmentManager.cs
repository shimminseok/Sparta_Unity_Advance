using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentItem : InventoryItem
{
    public bool IsEquipped;
    public InventorySlot LinkedSlot;
    public new EquipmentItemSO ItemSo => base.ItemSo as EquipmentItemSO;

    public EquipmentItem(EquipmentItemSO itemSo, InventorySlot linkedSlot) : base(itemSo, 1)
    {
        IsEquipped = false;
        LinkedSlot = linkedSlot;
    }
}

public class EquipmentManager : MonoBehaviour
{
    public Dictionary<EquipmentType, EquipmentItem> EquipmentItems { get; private set; } = new Dictionary<EquipmentType, EquipmentItem>();

    public event Action<EquipmentType> OnEquipmentChanged;

    public void EquipItem(EquipmentItem data)
    {
        EquipmentType type = data.ItemSo.EquipmentType;
        if (data.IsEquipped || EquipmentItems.ContainsKey(type))
        {
            UnEquipItem(type);
            if (data.IsEquipped)
                return;
        }

        EquipmentItems[type] = data;
        foreach (StatData stat in data.ItemSo.Stats)
        {
            PlayerController.Instance.StatManager.ApplyStatEffect(stat.StatType, StatModifierType.Equipment, stat.Value);
        }

        data.IsEquipped = true;
        data.LinkedSlot.SetEquipMark(true);
        Debug.Log($"아이템 장착 : {data.ItemSo.ItemName}");
        OnEquipmentChanged?.Invoke(type);
    }

    public void UnEquipItem(EquipmentType type)
    {
        if (EquipmentItems.ContainsKey(type) && EquipmentItems[type] != null)
        {
            EquipmentItem item = EquipmentItems[type];
            // InventoryManager.Instance.AddItem(item.ItemSo);
            foreach (StatData stat in item.ItemSo.Stats)
            {
                PlayerController.Instance.StatManager.ApplyStatEffect(stat.StatType, StatModifierType.Equipment, -stat.Value);
            }

            item.IsEquipped = false;
            item.LinkedSlot.SetEquipMark(false);
            EquipmentItems[type] = null;
            Debug.Log($"아이템 장착 해제 : {item.ItemSo.ItemName}");
            OnEquipmentChanged?.Invoke(type);
        }
    }
}