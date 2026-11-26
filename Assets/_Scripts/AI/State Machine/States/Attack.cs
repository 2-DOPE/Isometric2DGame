
using UnityEngine;

[CreateAssetMenu(fileName = "New_AttackState", menuName = "AI/States/Attack")]
public class Attack : State
{
    [SerializeField] private float attackRange = 1.5f;

    public override bool CanInterrupt(StateMachine machine)
    {
        AIStateMachine ai = (AIStateMachine)machine;

        if (ai.perceptionTarget == null)
            return false;

        float distance = Vector2.Distance(
            ai.Character.transform.position,
            ai.perceptionTarget.transform.position
        );

        return distance <= attackRange;
    }

    public override void Enter(StateMachine machine)
    {
        AIStateMachine ai = (AIStateMachine)machine;
        ai.Movement?.StopMovement();
    }

    public override StateStatus Tick(StateMachine machine)
    {
        AIStateMachine ai = (AIStateMachine)machine;

        if (ai.perceptionTarget == null)
            return StateStatus.Failed;

        // Execute combat behaviour here.
        CombatComponent combat =
            ai.Character.GetComponent<CombatComponent>();

        if (combat == null)
            return StateStatus.Failed;

        combat.Attack(ai.perceptionTarget);

        return StateStatus.Running;
    }

    public override void Exit(StateMachine machine)
    {
        AIStateMachine ai = (AIStateMachine)machine;
        ai.Movement?.StopMovement();
    }
}
