using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StatManager))]
[RequireComponent(typeof(StatusEffectManager))]
public class PlayerController : SceneOnlySingleton<PlayerController>
{
    public StatManager         StatManager         { get; private set; }
    public StatusEffectManager StatusEffectManager { get; private set; }
    public EquipmentManager    EquipmentManager    { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        StatManager = GetComponent<StatManager>();
        StatusEffectManager = GetComponent<StatusEffectManager>();
        EquipmentManager = GetComponent<EquipmentManager>();
    }

    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }


    public void TakeDamage(float amount)
    {
        StatManager.Consume(StatType.CurHp, amount);
        float curHp = StatManager.GetValue(StatType.CurHp);

        if (curHp <= 0)
        {
            print($"플레이어 사망");
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}