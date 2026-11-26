
using UnityEngine;

public enum StateStatus
{
    Running,
    Completed,
    Success,
    Failed
}

public abstract class State : ScriptableObject, IState
{
    protected StateMachine machine;

    public abstract void Enter(StateMachine machine);

    public abstract StateStatus Tick(StateMachine machine);

    public abstract void Exit(StateMachine machine);

    // Determines whether this state can interrupt the current state.
    public virtual bool CanInterrupt(StateMachine machine)
    {
        return false;
    }
}
