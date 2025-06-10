using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentItemSlot : MonoBehaviour
{
    [SerializeField] private EquipmentType equipmentType;
    [SerializeField] private Image equipmentItemIcon;
    [SerializeField] private TextMeshProUGUI equipmentItemEnhanceCountTxt;

    private EquipmentManager equipmentManager;
    private EquipmentItem equipmentItem;

    private GameManager GameManager => GameManager.Instance;

    private void Awake()
    {
        equipmentManager = GameManager.PlayerController.EquipmentManager;
    }

    private void Start()
    {
    }

    private void EmptySlot()
    {
        equipmentItemEnhanceCountTxt.text = string.Empty;
        equipmentItemIcon.enabled = false;
        equipmentItem = null;
    }

    public void EquipmentItem()
    {
        if (!equipmentManager.EquipmentItems.TryGetValue(equipmentType, out equipmentItem) || equipmentItem == null)
        {
            EmptySlot();
            return;
        }

        equipmentItemIcon.enabled = true;
        equipmentItem = equipmentManager.EquipmentItems[equipmentType];
        equipmentItemIcon.sprite = equipmentItem.ItemSo.ItemSprite;
        equipmentItem.OnItemChanged -= Refresh;
        equipmentItem.OnItemChanged += Refresh;
        Refresh();
    }

    private void Refresh()
    {
        equipmentItemEnhanceCountTxt.text = equipmentItem.GetEnhanceCountStr();
    }
}