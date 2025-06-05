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

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        InstantiateSlot();
    }

    private void Update()
    {
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
            statSlot.UpdateSlot(stat.Value);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}