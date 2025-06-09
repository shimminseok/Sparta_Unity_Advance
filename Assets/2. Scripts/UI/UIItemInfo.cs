using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UIItemInfo : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [SerializeField] private InventorySlot showItemSlot;
    [SerializeField] private TextMeshProUGUI showItemName;
    [SerializeField] private TextMeshProUGUI showItemDescription;
    [SerializeField] private GameObject enhanceBtnObj;


    private InventoryItem showItemData;

    private void Awake()
    {
        Close();
    }

    private void SetShowItemInfo(InventorySlot item)
    {
        showItemData = item.InventoryItem;
        enhanceBtnObj.SetActive(showItemData is EquipmentItem);
        showItemSlot.SetItem(item.Index, showItemData);
        showItemName.text = showItemData.ItemSo.ItemName;
        showItemDescription.text = showItemData.ItemSo.ItemDescription;
    }


    public void OnClickEnhanceBtn()
    {
        if (showItemData is not EquipmentItem equipmentItem)
            return;

        equipmentItem.Enhancement();
    }

    public void Open(InventorySlot itemData)
    {
        content.SetActive(true);
        SetShowItemInfo(itemData);
    }

    public void Close()
    {
        content.SetActive(false);
    }
}