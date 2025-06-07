using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UICharInfo : UIBase<UICharInfo>, IUIBase
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotRoot;
    [SerializeField] private EquipmentItemSlot[] equipmentSlots;

    private GameManager gameManager;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        InstantiateSlot();
        gameManager.PlayerController.EquipmentManager.OnEquipmentChanged += UpdateEquipmentSlot;
    }

    public override void Open()
    {
        base.Open();
    }

    public override void Close()
    {
        base.Close();
    }

    private void InstantiateSlot()
    {
        var statDic = gameManager.PlayerController.StatManager.PlayerStat;

        foreach (var stat in statDic)
        {
            if (stat.Value.Type == StatType.MaxHp)
                continue;

            GameObject obj      = Instantiate(slotPrefab, slotRoot);
            StatSlot   statSlot = obj.GetComponent<StatSlot>();
            statSlot.SetSlot(stat.Value);
            stat.Value.OnValueChanged += statSlot.UpdateSlot;
        }
    }

    private void UpdateEquipmentSlot(EquipmentType equipmentType)
    {
        equipmentSlots[(int)equipmentType].EquipmentItem();
    }

    protected void OnDisable()
    {
        if (gameManager && gameManager.PlayerController.EquipmentManager)
            gameManager.PlayerController.EquipmentManager.OnEquipmentChanged -= UpdateEquipmentSlot;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}