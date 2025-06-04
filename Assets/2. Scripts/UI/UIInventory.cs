using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIInventory : UIBase<UIInventory>, IUIBase
{
    [SerializeField] private GameObject inventorySlotPrefabs;
    [SerializeField] private Transform scrollviewContent;



    private InventorySlot[] inventorySlots;
    private InventoryManager inventoryManager;
    
    public InventorySlot SelectedItem { get; private set; }
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        inventoryManager = InventoryManager.Instance;
        InitializeSlots();
        inventoryManager.OnInventorySlotUpdate += UpdateInventorySlot;
    }

    private void InitializeSlots()
    {
        inventorySlots = new InventorySlot[inventoryManager.Inventory.Count];
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            GameObject obj = Instantiate(inventorySlotPrefabs, scrollviewContent);
            inventorySlots[i] = obj.GetComponent<InventorySlot>();
            inventorySlots[i].SetItem(i, InventoryManager.Instance.Inventory[i]);
        }
    }

    public void SelecteItem(InventorySlot item)
    {
        if (SelectedItem != null && SelectedItem != item)
            SelectedItem.DeSelectedSlot();

        SelectedItem = item;
        ShowItemInfo();
    }

    private void UpdateInventorySlot(int index)
    {
        if (index < 0 || index >= inventorySlots.Length)
            return;

        InventoryItem itemData = InventoryManager.Instance.Inventory[index];

        inventorySlots[index].SetItem(index, itemData);
    }

    private void ShowItemInfo()
    {
        if (SelectedItem == null)
            return;
    }

    public override void Open()
    {
        base.Open();
    }

    public override void Close()
    {
        SelectedItem?.DeSelectedSlot();
        SelectedItem = null;
        base.Close();
    }
}