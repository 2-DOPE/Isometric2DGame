
using UnityEngine;


[CreateAssetMenu(fileName = "New_PatrolState", menuName = "AI/States/Patrol")]
public class PatrolState : State
{
    [Header("Patrol Settings")]
    [Tooltip("How close the character needs to get to a waypoint.")]
    public float stoppingDistance = 0.15f;

    [Tooltip("Time to wait at a waypoint before moving to the next.")]
    public float WaitTime = 5f;

    private float waitTimer;
    private bool isWaiting;

    public override void Enter(StateMachine machine)
    {
        AIStateMachine ai = (AIStateMachine)machine;

        ai.currentWaypointIndex = 0;

        waitTimer = 0f;
        isWaiting = false;
    }

    public override StateStatus Tick(StateMachine machine)
    {
        AIStateMachine ai = (AIStateMachine)machine;

        if (ai.Movement == null ||
            ai.waypoints == null ||
            ai.waypoints.Length == 0)
            return StateStatus.Failed;

        if (ai.currentWaypointIndex >= ai.waypoints.Length)
        {
            ai.Movement.StopMovement();
            return StateStatus.Completed;
        }

        Transform targetWaypoint = ai.waypoints[ai.currentWaypointIndex];

        if (targetWaypoint == null)
        {
            ai.Movement.StopMovement();
            return StateStatus.Failed;
        }

        Vector2 currentPos = ai.Character.transform.position;
        Vector2 targetPos = targetWaypoint.position;
        Vector2 toTarget = targetPos - currentPos;

        if (toTarget.magnitude <= stoppingDistance)
        {
            ai.Movement.StopMovement();

            if (!isWaiting)
            {
                isWaiting = true;
                waitTimer = WaitTime;
            }

            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0f)
            {
                isWaiting = false;
                ai.currentWaypointIndex++;

                if (ai.currentWaypointIndex >= ai.waypoints.Length)
                    return StateStatus.Completed;
            }
        }
        else
        {
            ai.Movement.SetMoveDirection(toTarget.normalized);
        }

        return StateStatus.Running;
    }
    public override void Exit(StateMachine machine)
    {
        AIStateMachine ai = (AIStateMachine)machine;

        isWaiting = false;
        ai.Movement?.StopMovement();
    }
}
