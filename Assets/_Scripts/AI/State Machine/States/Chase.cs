
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New_ChaseState", menuName = "AI/States/Chase")]
public class Chase : State
{
    [SerializeField] private float chaseRange = 15f;
    [SerializeField] private float chaseMaxSpeed = 15f;

    public override bool CanInterrupt(StateMachine machine)
    {
        AIStateMachine ai = (AIStateMachine)machine;

        // Require a perceived target.
        if (ai.perceptionTarget == null)
            return false;

        // Check target distance.
        float distance = Vector2.Distance(
            ai.Character.transform.position,
            ai.perceptionTarget.transform.position
        );

        return distance <= chaseRange;
    }

    public override void Enter(StateMachine machine)
    {
        AIStateMachine ai = (AIStateMachine)machine;
        ai.SelectTarget();
    }

    public override StateStatus Tick(StateMachine machine)
    {
        AIStateMachine ai = (AIStateMachine)machine;

        if (ai.perceptionTarget == null)
            return StateStatus.Failed;

        if (!ai.Perception.GetPerceivedActors().Contains(ai.perceptionTarget))
            return StateStatus.Failed;

        Vector2 direction =
            (Vector2)(ai.perceptionTarget.transform.position -
                      ai.Character.transform.position);

        ai.Movement.MoveSpeed.Equals(chaseMaxSpeed/ Vector2.Distance(ai.Character.transform.position,ai.perceptionTarget.transform.position)); 
        ai.Movement.SetMoveDirection(direction.normalized);

        return StateStatus.Running;
    }

    public override void Exit(StateMachine machine)
    {
        AIStateMachine ai = (AIStateMachine)machine;
        //Stop Movement
        ai.Movement?.StopMovement();
        //Reset Movement Speed To Default
        ai.Movement.MoveSpeed.Equals(default);

    }
}