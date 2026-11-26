
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct StateTransition
{
    public State fromState;
    public State onCompletedState;
    public State onFailedState;
}

[Serializable]
public struct InterruptTransition
{
    public State targetState;

    [Tooltip("Higher values are evaluated first.")]
    public int priority;
}

public abstract class StateMachine : MonoBehaviour
{
    [Header("State Configuration")]
    [SerializeField] private State initialState;
    [SerializeField] private List<StateTransition> transitions = new();

    [Header("Interrupt Configuration")]
    [SerializeField] private List<InterruptTransition> interrupts = new();

    public State CurrentState { get; private set; }

    private bool initialized;

    public virtual void Initialize()
    {
        initialized = true;
        ChangeState(initialState);
    }
    protected virtual void BeforeTick() { }
    public void Tick()
    {
        if (!initialized || CurrentState == null)
            return;
        BeforeTick();

        // Check whether a higher-priority behaviour can interrupt.
        if (EvaluateInterruptions())
            return;

        StateStatus status = CurrentState.Tick(this);

        switch (status)
        {
            case StateStatus.Completed:
            case StateStatus.Success:
                TranslateTo(CurrentState, true);
                break;

            case StateStatus.Failed:
                TranslateTo(CurrentState, false);
                break;
        }
    }

    // Evaluate interrupt states by priority.
    private bool EvaluateInterruptions()
    {
        State bestState = null;
        int highestPriority = int.MinValue;

        foreach (InterruptTransition interrupt in interrupts)
        {
            State candidate = interrupt.targetState;

            if (candidate == null || candidate == CurrentState)
                continue;

            if (interrupt.priority <= highestPriority)
                continue;

            // Ask the candidate state if its conditions are satisfied.
            if (!candidate.CanInterrupt(this))
                continue;

            bestState = candidate;
            highestPriority = interrupt.priority;
        }

        if (bestState == null)
            return false;

        ChangeState(bestState);
        return true;
    }

    public void TranslateTo(State finishedState, bool succeeded)
    {
        foreach (StateTransition transition in transitions)
        {
            if (transition.fromState != finishedState)
                continue;

            State nextState = succeeded
                ? transition.onCompletedState
                : transition.onFailedState;

            if (nextState != null)
            {
                ChangeState(nextState);
                Debug.Log($"{name}: Switching to {nextState.name}");
            }
            else if (!succeeded)
            {
                Debug.LogWarning(
                    $"{name}: {finishedState.name} failed, but no fallback was assigned."
                );
            }

            return;
        }
    }

    public void ChangeState(State newState)
    {
        if (newState == null || newState == CurrentState)
            return;

        CurrentState?.Exit(this);

        CurrentState = newState;
        CurrentState.Enter(this);
    }

    public virtual void Shutdown()
    {
        CurrentState?.Exit(this);
        CurrentState = null;
        initialized = false;
    }
}
