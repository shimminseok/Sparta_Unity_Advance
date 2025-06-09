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

        public void OnExit(PlayerController entity)
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
            if (owner.Agent.remainingDistance <= owner.Agent.stoppingDistance)
                return PlayerState.Attack;
            else if (owner.Target == null || owner.Target.IsDead)
                return PlayerState.Idle;

            return PlayerState.Move;
        }
    }

    public class AttackState : IState<PlayerController, PlayerState>
    {
        private float attackTimer;
        private readonly float attackDelay;

        public AttackState(float atkSpd)
        {
            attackDelay = atkSpd;
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
            if (owner.Target == null || owner.Target.IsDead)
                return PlayerState.Idle;
            else if (owner.Agent.remainingDistance > owner.Agent.stoppingDistance)
                return PlayerState.Move;


            return PlayerState.Attack;
        }
    }
}