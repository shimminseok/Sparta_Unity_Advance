using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UICharInfo : UIBase<UICharInfo>, IUIBase
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotRoot;
    [SerializeField] private EquipmentItemSlot[] equipmentSlots;

    private GameManager gameManager => GameManager.Instance;
    private List<StatSlot> statSlots = new List<StatSlot>();

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
    }

    public override void Open()
    {
        base.Open();
        if (statSlots.Count == 0)
        {
            InstantiateSlot();
        }

        var statDic = gameManager.PlayerController.StatManager.Stats;
        int index   = 0;
        foreach (var stat in statDic)
        {
            if (stat.Value.Type == StatType.MaxHp || stat.Value.Type == StatType.MaxMp)
                continue;


            stat.Value.OnValueChanged += statSlots[index].UpdateSlot;
            statSlots[index].UpdateSlot(stat.Value.Value);
        }

        foreach (EquipmentItemSlot equipmentItemSlot in equipmentSlots)
        {
            equipmentItemSlot.EquipmentItem();
        }

        gameManager.PlayerController.EquipmentManager.OnEquipmentChanged += UpdateEquipmentSlot;
    }

    public override void Close()
    {
        base.Close();
        var statDic = gameManager.PlayerController.StatManager.Stats;
        int index   = 0;
        foreach (var stat in statDic)
        {
            if (stat.Value.Type == StatType.MaxHp || stat.Value.Type == StatType.MaxMp)
                continue;

            stat.Value.OnValueChanged -= statSlots[index].UpdateSlot;
        }

        gameManager.PlayerController.EquipmentManager.OnEquipmentChanged -= UpdateEquipmentSlot;
    }

    private void InstantiateSlot()
    {
        var statDic = gameManager.PlayerController.StatManager.Stats;

        foreach (var stat in statDic)
        {
            if (stat.Value.Type == StatType.MaxHp || stat.Value.Type == StatType.MaxMp)
                continue;

            GameObject obj      = Instantiate(slotPrefab, slotRoot);
            StatSlot   statSlot = obj.GetComponent<StatSlot>();
            statSlot.SetSlot(stat.Value);
            statSlots.Add(statSlot);
        }
    }

    private void UpdateEquipmentSlot(EquipmentType equipmentType)
    {
        equipmentSlots[(int)equipmentType].EquipmentItem();
    }

    protected void OnDisable()
    {
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}