using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayerStates;
using UnityEngine;

[RequireComponent(typeof(EquipmentManager))]
[RequireComponent(typeof(InputHandler))]
public class PlayerController : BaseController<PlayerController, PlayerState>, IAttackable, IDamageable
{
    public EquipmentManager EquipmentManager { get; private set; }
    public InputHandler     InputHandler     { get; private set; }
    public StatBase         AttackStat       { get; private set; }
    public IDamageable      Target           { get; private set; }
    public bool             IsDead           { get; private set; }
    public Transform        Transform        => transform;


    protected override void Awake()
    {
        base.Awake();
        PlayerTable playerTable = TableManager.Instance.GetTable<PlayerTable>();
        PlayerSO    playerData  = playerTable.GetDataByID(1);
        StatManager.Initialize(playerData);
        GameManager.Instance.SetPlayerController(this);
        EquipmentManager = GetComponent<EquipmentManager>();
        InputHandler = GetComponent<InputHandler>();
    }

    protected override void Start()
    {
        AttackStat = StatManager.GetStat<CalculatedStat>(StatType.AttackPow);
        Agent.stoppingDistance = StatManager.GetValue(StatType.AttackRange);
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override IState<PlayerController, PlayerState> GetState(PlayerState state)
    {
        return state switch
        {
            PlayerState.Idle   => new IdleState(),
            PlayerState.Move   => new MoveState(),
            PlayerState.Attack => new AttackState(StatManager.GetValue(StatType.AttackSpd), StatManager.GetValue(StatType.AttackRange)),
            _                  => null
        };
    }

    public override void Movement()
    {
        Agent.speed = StatManager.GetValue(StatType.MoveSpeed);
        if (Target != null && !Target.IsDead)
        {
            Agent.SetDestination(Target.Transform.position);
        }
    }

    public void Attack()
    {
        Target?.TakeDamage(this);
    }

    public override void FindTarget()
    {
        var enemies = EnemyManager.Instance.Enemies;
        if (enemies.Count == 0)
            return;

        enemies = enemies.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).ToList();
        Target = enemies[0];
    }

    public void TakeDamage(IAttackable attacker)
    {
        if (Target == null)
            Target = attacker as IDamageable;


        GameManager.Instance.MainCameraShake();
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
        print($"플레이어 사망");
    }
}