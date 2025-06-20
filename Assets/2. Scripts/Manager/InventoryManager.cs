using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
    public ItemSO ItemSo;
    public int Quantity;

    public InventoryItem()
    {
    }

    public InventoryItem(ItemSO itemSo, int quantity)
    {
        ItemSo = itemSo;
        Quantity = quantity;
    }

    public event Action OnItemChanged;

    public virtual InventoryItem Clone() => new InventoryItem(ItemSo, Quantity);

    public void ChangeQuantity(int amount)
    {
        Quantity += amount;
        ItemChanged();
    }

    public void ItemChanged()
    {
        OnItemChanged?.Invoke();
    }
}

[Serializable]
public class SaveInventoryItem
{
    public int Id;
    public int Quantity;

    public SaveInventoryItem(InventoryItem item)
    {
        Id = item.ItemSo.ID;
        Quantity = item.Quantity;
    }

    public SaveInventoryItem()
    {
    }
}

public class InventoryManager : SceneOnlySingleton<InventoryManager>
{
    [SerializeField] private int inventorySize;
    public List<InventoryItem> Inventory { get; private set; }


    public event Action<int> OnInventorySlotUpdate;

    private GameManager gameManager;

    protected override void Awake()
    {
        base.Awake();
        gameManager = GameManager.Instance;
        InitInventory();
    }

    private void InitInventory()
    {
        Inventory = new List<InventoryItem>(Enumerable.Repeat<InventoryItem>(null, inventorySize));
    }

    private void RemoveItem(int index)
    {
        Inventory[index] = null;
        OnInventorySlotUpdate?.Invoke(index);
    }

    public void AddItem(InventoryItem item, int amount = 1)
    {
        if (item.ItemSo is ConsumableItemSO consumableItemSo && consumableItemSo.IsStackable)
        {
            AddStackableItem(item, amount);
        }
        else
            AddNonStackableItem(item, amount);
    }

    /// <summary>
    /// 스택형 아이템을 추가하는 함수
    /// </summary>
    /// <param name="itemSo"></param>
    /// <param name="amount"></param>
    private void AddStackableItem(InventoryItem item, int amount = 1)
    {
        InventoryItem findItem = Inventory.Find(x => x != null && x.ItemSo.ID == item.ItemSo.ID);

        if (findItem == null)
        {
            // To Do 인벤토리가 꽉찼는지 확인
            int index = Inventory.IndexOf(null);
            if (index < 0)
            {
                Debug.Log("인벤토리 공간이 부족합니다.");
                return;
            }
            else
            {
                findItem = new InventoryItem(item.ItemSo, amount);
            }

            Inventory[index] = findItem;
            OnInventorySlotUpdate?.Invoke(index);
        }
        else
        {
            findItem.ChangeQuantity(amount);
        }
    }

    /// <summary>
    /// 비스택형 아이템을 추가하는 함수
    /// </summary>
    /// <param name="itemSo"></param>
    /// <param name="amount"></param>
    private void AddNonStackableItem(InventoryItem item, int amount = 1)
    {
        for (int i = 0; i < amount; i++)
        {
            int index = Inventory.IndexOf(null);
            if (index < 0)
                return;
            Inventory[index] = item.Clone();
            OnInventorySlotUpdate?.Invoke(index);
        }
    }

    public void UseItem(InventoryItem item, int amount = 1)
    {
        int index = Inventory.IndexOf(item);

        if (index < 0 || item.Quantity < amount) return;
        if (item.ItemSo is not ConsumableItemSO consumableItemSo)
            return;
        foreach (StatusEffectData itemSoStatusEffect in consumableItemSo.StatusEffects)
        {
            gameManager.PlayerController.StatusEffectManager.ApplyEffect(BuffFactory.CreateBuff(itemSoStatusEffect));
        }

        item.ChangeQuantity(-amount);
        if (item.Quantity <= 0)
            RemoveItem(index);
    }

    public void DropItem(int index, int amount)
    {
        InventoryItem data = Inventory[index];
        if (data == null || data.Quantity < amount)
            return;

        data.ChangeQuantity(-amount);
        if (data.Quantity == 0)
            RemoveItem(index);
    }


    public void SwichItem(int from, int to)
    {
        (Inventory[from], Inventory[to]) = (Inventory[to], Inventory[from]);

        OnInventorySlotUpdate?.Invoke(from);
        OnInventorySlotUpdate?.Invoke(to);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}