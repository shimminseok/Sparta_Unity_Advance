using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cheat : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }


    private void OnGUI()
    {
#if UNITY_EDITOR
        if (GUI.Button(new Rect(50f, 50f, 250f, 150f), "골드 증가"))
        {
            AccountManager.Instance.AddGold(100000);
        }

        if (GUI.Button(new Rect(50f, 200f, 250f, 150f), "아이템 획득"))
        {
            foreach (var data in TableManager.Instance.GetTable<ItemTable>().dataDic.Values)
            {
                int amount = 1;
                if (data is ConsumableItemSO consumable)
                {
                    amount = consumable.MaxStack;
                    InventoryItem item = new InventoryItem(data, amount);
                    InventoryManager.Instance.AddItem(item, amount);
                }
                else if (data is EquipmentItemSO equipment)
                {
                    EquipmentItem equipmentItem = new EquipmentItem(equipment, null);
                    InventoryManager.Instance.AddItem(equipmentItem, amount);
                }
            }
        }

        if (GUI.Button(new Rect(50f, 350f, 250f, 150f), "대미지"))
        {
            gameManager.PlayerController.TakeDamage(30);
        }
#endif
    }
}