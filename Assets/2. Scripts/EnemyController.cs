using System;
using System.Collections;
using System.Collections.Generic;
using EnemyStates;
using UnityEngine;


public class EnemyController : BaseController<EnemyController, EnemyState>, IAttackable, IDamageable
{
    public StatBase    AttackStat { get; private set; }
    public IDamageable Target     { get; private set; }
    public bool        IsDead     { get; private set; }
    public Transform   Transform  => transform;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        AttackStat = StatManager.GetStat<CalculatedStat>(StatType.AttackPow);
        Agent.stoppingDistance = StatManager.GetValue(StatType.AttackRange);
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
            EnemyState.Attack => new AttackState(StatManager.GetValue(StatType.AttackSpd), StatManager.GetValue(StatType.AttackRange)),
            _                 => null
        };
    }

    public override void FindTarget()
    {
        if (Target != null && Target.IsDead)
            return;

        Target = GameManager.Instance.PlayerController;
    }

    public override void Movement()
    {
        if (Target != null)
        {
            Agent.speed = StatManager.GetValue(StatType.MoveSpeed);
            Agent.SetDestination(Target.Transform.position);
        }
    }


    public void Attack()
    {
        Target?.TakeDamage(this);
    }

    public void TakeDamage(IAttackable attacker)
    {
        if (Target == null)
            Target = attacker as IDamageable;

        StatManager.Consume(StatType.CurHp, attacker.AttackStat.Value);

        float curHp = StatManager.GetValue(StatType.CurHp);
        if (curHp <= 0)
        {
            Daed();
        }
    }

    public void Daed()
    {
        IsDead = true;
        EnemyManager.Instance.MonsterDead(this);
        print($"몬스터 사망");
    }
}