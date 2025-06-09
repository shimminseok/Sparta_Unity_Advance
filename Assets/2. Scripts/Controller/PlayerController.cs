using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _10._Tables.ScriptableObj;
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
        GameManager.Instance.SetPlayerController(this);
        EquipmentManager = GetComponent<EquipmentManager>();
        InputHandler = GetComponent<InputHandler>();
    }

    protected override void Start()
    {
        PlayerTable playerTable = TableManager.Instance.GetTable<PlayerTable>();
        PlayerSO    playerData  = playerTable.GetDataByID(1);
        StatManager.Initialize(playerData);
        AttackStat = StatManager.GetStat<CalculatedStat>(StatType.AttackPow);
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
            PlayerState.Attack => new AttackState(StatManager.GetValue(StatType.AttackSpd)),
            _                  => null
        };
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


    // 테스트
    public void TakeDamage(float amount)
    {
        StatManager.Consume(StatType.CurHp, amount);
        float curHp = StatManager.GetValue(StatType.CurHp);
        if (curHp <= 0)
        {
            Daed();
        }
    }
}