using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            foreach (var data in TableManager.Instance.GetTable<ItemTable>().dataDic.Values)
            {
                InventoryManager.Instance.AddItem(data);
            }
        }
    }
}