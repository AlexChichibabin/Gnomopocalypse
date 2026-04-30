using System;
using System.Collections.Generic;

public abstract class StateMachine
{
    private Dictionary<Type, object> states;

    private object currentState;
    public object CurrentState => currentState;

    public StateMachine()
    {
        states = new Dictionary<Type, object>();
    }

    public void AddState<TState>(TState state) where TState : class, IState
    {
        states.Add(state.GetType(), state);
    }
    public void RemoveState<TState>() where TState : class, IState
    {
        states.Remove(typeof(TState));
    }
    public void Enter<TState>() where TState : class, IState
    {
        if (currentState != null && typeof(TState) == currentState.GetType()) return;

        if (currentState is IExitableState exitableState) exitableState.Exit();

        TState state = states[typeof(TState)] as TState;

        currentState = state;

        if (currentState is IEnterableState enterableState) enterableState.Enter();
    }
    public void Exit<TState>() where TState : class, IState
    {
        if (currentState is IExitableState exitableState) exitableState.Exit();

        currentState = null;
    }
    public void UpdateTick()
    {
        if (currentState is ITickableState tickableState) tickableState.Tick();
    }
}