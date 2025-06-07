using System;
using System.Collections;
using System.Collections.Generic;
using PlayerStates;
using UnityEngine;

public abstract class BaseController<TController, TState> : MonoBehaviour where TController : BaseController<TController, TState> where TState : Enum
{
    public StatManager         StatManager         { get; private set; }
    public StatusEffectManager StatusEffectManager { get; private set; }
    protected StateMachine<TController, TState> stateMachine;
    protected IState<TController, TState>[] states;
    private TState currentState;

    protected virtual void Awake()
    {
        StatManager = GetComponent<StatManager>();
        StatusEffectManager = GetComponent<StatusEffectManager>();
        stateMachine = new StateMachine<TController, TState>();
    }

    protected virtual void Start()
    {
        SetupState();
    }

    protected virtual void Update()
    {
        TryStateTransition();
        stateMachine?.Update();
    }

    protected virtual void FixedUpdate()
    {
        stateMachine?.FixedUpdate();
    }

    private void SetupState()
    {
        Array values = Enum.GetValues(typeof(TState));
        states = new IState<TController, TState>[values.Length];
        for (int i = 0; i < states.Length; i++)
        {
            TState state = (TState)values.GetValue(i);
            states[i] = GetState(state);
        }

        currentState = (TState)values.GetValue(0);
        stateMachine.Setup((TController)this, states[0]);
    }

    protected abstract IState<TController, TState> GetState(TState state);

    private void ChangeState(TState newState)
    {
        stateMachine.ChangeState(states[Convert.ToInt32(newState)]);
        currentState = newState;
    }

    private void TryStateTransition()
    {
        int currentIndex = Convert.ToInt32(currentState);
        var next         = states[currentIndex].CheckTransition((TController)this);
        if (!next.Equals(currentState))
        {
            ChangeState(next);
        }
    }
}