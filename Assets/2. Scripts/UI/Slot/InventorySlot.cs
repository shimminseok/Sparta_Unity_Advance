using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemQuantity;
    [SerializeField] private TextMeshProUGUI equipMark;
    public InventoryItem InventoryItem { get; private set; }
    private bool isSelected;

    public bool IsEmpty => InventoryItem == null;
    public int  Index   { get; private set; }

    private GameManager gameManager => GameManager.Instance;


    public void SetItem(int index, InventoryItem item)
    {
        Index = index;
        if (item == null || item.Quantity == 0)
        {
            EmptySlot();
            return;
        }

        icon.enabled = true;
        InventoryItem = item;
        icon.sprite = item.ItemSo.ItemSprite;
        itemQuantity.text = item.Quantity > 1 ? $"x{item.Quantity}" : string.Empty;

        if (item.ItemSo is EquipmentItemSO equipmentItemSo)
        {
            if (gameManager.PlayerController.EquipmentManager.EquipmentItems.TryGetValue(equipmentItemSo.EquipmentType, out var equipitem))
            {
                SetEquipMark(equipitem != null && equipitem == InventoryItem);
            }
        }
        else
        {
            SetEquipMark(false);
        }
    }

    public void EmptySlot()
    {
        icon.enabled = false;
        InventoryItem = null;
        itemQuantity.text = "";

        SetEquipMark(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsEmpty || !EventSystem.current.IsPointerOverGameObject())
            return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (isSelected)
                DeSelectedSlot();
            else
                SelectedSlot();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            UseOrEquipItem();
        }
    }

    private void SelectedSlot()
    {
        UIInventory.Instance.SelecteItem(this);
        isSelected = true;
    }

    public void DeSelectedSlot()
    {
        isSelected = false;
    }

    private void UseOrEquipItem()
    {
        if (InventoryItem is EquipmentItem equipmentItem)
        {
            var equipmentManager = gameManager.PlayerController.EquipmentManager;
            equipmentItem.LinkedSlot = this;
            equipmentManager.EquipItem(equipmentItem);
            return;
        }

        InventoryManager.Instance.UseItem(InventoryItem, 1);
    }

    public void SetEquipMark(bool isActive)
    {
        equipMark.gameObject.SetActive(isActive);
    }

    private void SwitchInvenSlot(InventorySlot swich)
    {
        InventoryManager.Instance.SwichItem(swich.Index, Index);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!DragManager.Instance.IsDragging)
            return;

        if (DragManager.Instance.DraggedInventoryItem != null)
        {
            SwitchInvenSlot(DragManager.Instance.DraggedInventoryItem);
        }
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            DragManager.Instance.StartDrag(this, UIInventory.Instance.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        DragManager.Instance.UpdateDrag(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragManager.Instance.EndDrag();
    }
}