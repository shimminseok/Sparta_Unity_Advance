using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerStates
{
    public class IdleState : IState<PlayerController, PlayerState>
    {
        public void OnEnter(PlayerController owner)
        {
        }

        public void OnUpdate(PlayerController owner)
        {
            owner.FindTarget();
        }

        public void OnFixedUpdate(PlayerController owner)
        {
        }

        public void OnExit(PlayerController owner)
        {
        }

        public PlayerState CheckTransition(PlayerController owner)
        {
            if (owner.Target != null)
                return PlayerState.Move;

            return PlayerState.Idle;
        }
    }

    public class MoveState : IState<PlayerController, PlayerState>
    {
        public void OnEnter(PlayerController owner)
        {
        }

        public void OnUpdate(PlayerController owner)
        {
            owner.Movement();
        }

        public void OnFixedUpdate(PlayerController owner)
        {
        }

        public void OnExit(PlayerController entity)
        {
        }

        public PlayerState CheckTransition(PlayerController owner)
        {
            IDamageable target = owner.Target;

            if (target == null || target.IsDead)
                return PlayerState.Idle;

            float distance = Vector3.Distance(owner.transform.position, target.Transform.position);

            if (distance <= owner.StatManager.GetValue(StatType.AttackRange))
                return PlayerState.Attack;

            return PlayerState.Move;
        }
    }

    public class AttackState : IState<PlayerController, PlayerState>
    {
        private float attackTimer;
        private readonly float attackDelay;
        private readonly float attackRange;

        public AttackState(float atkSpd, float atkRange)
        {
            attackDelay = atkSpd;
            attackRange = atkRange;
        }

        public void OnEnter(PlayerController owner)
        {
        }

        public void OnUpdate(PlayerController owner)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackDelay)
            {
                attackTimer = 0f;
                owner.Attack();
            }
        }

        public void OnFixedUpdate(PlayerController owner)
        {
        }

        public void OnExit(PlayerController entity)
        {
            attackTimer = 0f;
        }

        public PlayerState CheckTransition(PlayerController owner)
        {
            IDamageable target = owner.Target;

            if (target == null || target.IsDead)
                return PlayerState.Idle;

            float distance = Vector3.Distance(owner.transform.position, target.Transform.position);

            return distance > owner.StatManager.GetValue(StatType.AttackRange)
                ? PlayerState.Move
                : PlayerState.Attack;
        }
    }
}