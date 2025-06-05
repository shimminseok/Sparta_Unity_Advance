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


    private void Awake()
    {
        equipmentManager = PlayerController.Instance.EquipmentManager;
    }

    private void Start()
    {
    }

    private void EmptySlot()
    {
        equipmentItemIcon.enabled = false;
        equipmentItem = null;
    }

    public void EquipmentItem()
    {
        if (equipmentManager.EquipmentItems[equipmentType] == null)
        {
            EmptySlot();
            return;
        }

        equipmentItemIcon.enabled = true;
        equipmentItem = equipmentManager.EquipmentItems[equipmentType];
        equipmentItemIcon.sprite = equipmentItem.ItemSo.ItemSprite;
    }
}