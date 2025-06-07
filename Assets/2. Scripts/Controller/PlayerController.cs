using System;
using System.Collections;
using System.Collections.Generic;
using PlayerStates;
using UnityEngine;

[RequireComponent(typeof(StatManager))]
[RequireComponent(typeof(StatusEffectManager))]
public class PlayerController : BaseController<PlayerController, PlayerState>
{
    public EquipmentManager EquipmentManager { get; private set; }

    //StateMachine
    private StateMachine<PlayerController, PlayerState> stateMachine;
    private IState<PlayerController, PlayerState>[] states;
    PlayerState currentState;

    protected override void Awake()
    {
        base.Awake();
        GameManager.Instance.SetPlayerController(this);
        EquipmentManager = GetComponent<EquipmentManager>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override IState<PlayerController, PlayerState> GetState(PlayerState state)
    {
        return state switch
        {
            PlayerState.Idle   => new IdleState(),
            PlayerState.Move   => new MoveState(),
            PlayerState.Attack => new AttackState(),
            _                  => null
        };
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
}