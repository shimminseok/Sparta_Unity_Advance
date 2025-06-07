using System;
using System.Collections;
using System.Collections.Generic;
using EnemyStates;
using UnityEngine;


public class EnemyController : BaseController<EnemyController, EnemyState>
{
    protected override void Awake()
    {
        base.Awake();
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


    protected override IState<EnemyController, EnemyState> GetState(EnemyState state)
    {
        return state switch
        {
            EnemyState.Idle   => new IdleState(),
            EnemyState.Move   => new MoveState(),
            EnemyState.Attack => new AttackState(),
            _                 => null
        };
    }
}