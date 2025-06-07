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
        }

        public void OnFixedUpdate(PlayerController owner)
        {
        }

        public void OnExit(PlayerController entity)
        {
        }

        public PlayerState CheckTransition(PlayerController owner)
        {
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
        }

        public void OnFixedUpdate(PlayerController owner)
        {
        }

        public void OnExit(PlayerController entity)
        {
        }

        public PlayerState CheckTransition(PlayerController owner)
        {
            return PlayerState.Idle;
        }
    }

    public class AttackState : IState<PlayerController, PlayerState>
    {
        public void OnEnter(PlayerController owner)
        {
        }

        public void OnUpdate(PlayerController owner)
        {
        }

        public void OnFixedUpdate(PlayerController owner)
        {
        }

        public void OnExit(PlayerController entity)
        {
        }

        public PlayerState CheckTransition(PlayerController owner)
        {
            return PlayerState.Idle;
        }
    }
}