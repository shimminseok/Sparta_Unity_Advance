using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentItemSlot : MonoBehaviour
{
    [SerializeField] private EquipmentType equipmentType;
    [SerializeField] private Image equipmentItemIcon;

    private EquipmentManager equipmentManager;
    private EquipmentItem equipmentItem;

    private GameManager gameManager => GameManager.Instance;

    private void Awake()
    {
        equipmentManager = gameManager.PlayerController.EquipmentManager;
    }

    private void EmptySlot()
    {
        equipmentItemIcon.enabled = false;
        equipmentItem = null;
    }

    public void EquipmentItem()
    {
        if (equipmentManager.EquipmentItems.TryGetValue(equipmentType, out equipmentItem))
        {
            if (equipmentItem == null)
            {
                EmptySlot();
                return;
            }
        }

        equipmentItemIcon.enabled = true;
        equipmentItem = equipmentManager.EquipmentItems[equipmentType];
        equipmentItemIcon.sprite = equipmentItem.ItemSo.ItemSprite;
    }
}