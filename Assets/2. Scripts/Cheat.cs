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
        float buttonWidth  = 100f;
        float buttonHeight = 30f;
        float spacing      = 5f;

        float x = 10f;
        float y = Screen.height - buttonHeight - 50f;

#if UNITY_EDITOR
        if (GUI.Button(new Rect(x, y, buttonWidth, buttonHeight), "골드 증가"))
        {
            AccountManager.Instance.AddGold(100000);
        }

        if (GUI.Button(new Rect(x, y - (buttonHeight + spacing), buttonWidth, buttonHeight), "아이템 획득"))
        {
            foreach (var data in TableManager.Instance.GetTable<ItemTable>().DataDic.Values)
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

        if (GUI.Button(new Rect(x, y - (buttonHeight + spacing) * 2, buttonWidth, buttonHeight), "대미지"))
        {
            gameManager.PlayerController.TakeDamage(30);
        }
#endif
    }
}