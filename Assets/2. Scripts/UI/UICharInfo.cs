using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICharInfo : UIBase<UICharInfo>, IUIBase
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotRoot;
    [SerializeField] private EquipmentItemSlot[] equipmentSlots;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        InstantiateSlot();
        PlayerController.Instance.EquipmentManager.OnEquipmentChanged += UpdateEquipmentSlot;
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
        var statDic = PlayerController.Instance.StatManager.PlayerStat;

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
        if (PlayerController.Instance && PlayerController.Instance.EquipmentManager)
            PlayerController.Instance.EquipmentManager.OnEquipmentChanged -= UpdateEquipmentSlot;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}