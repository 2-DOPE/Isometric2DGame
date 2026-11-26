using UnityEngine;
using System.Linq;

public class AIStateMachine : StateMachine
{
    public AIController Controller { get; private set; }
    public Character Character { get; private set; }
    public IMoveable Movement { get; private set; }
    public PerceptionComponent Perception { get; private set; }

    [Header("Patrol Waypoints")]
    public Transform[] waypoints;

    [HideInInspector] public int currentWaypointIndex;
    [HideInInspector] public GameObject perceptionTarget;

    // Select the first currently perceived actor.
    public void SelectTarget()
    {
        perceptionTarget = Perception?
            .GetPerceivedActors()
            .FirstOrDefault();

    }

    public void Setup(AIController controller, Character character)
    {
        Controller = controller;
        Character = character;
        Movement = character.GetComponent<IMoveable>();
        Perception = character.GetComponent<PerceptionComponent>();

        if (Movement == null)
            Debug.LogWarning($"{character.name} has no movement component.");

        if (Perception == null)
            Debug.LogWarning($"{character.name} has no perception component.");
        else
            Perception.BeginPerception();
    }
    protected override void BeforeTick()
    {
        // Keep the current target if it's still perceived.
        if (perceptionTarget != null &&
            Perception.GetPerceivedActors().Contains(perceptionTarget))
            return;

        // Otherwise select another perceived actor.
        perceptionTarget = Perception?
            .GetPerceivedActors()
            .FirstOrDefault();
    }
    public override void Shutdown()
    {
        Movement?.StopMovement();
        Perception?.EndPerception();

        base.Shutdown();

        Controller = null;
        Character = null;
        Movement = null;
        Perception = null;
        perceptionTarget = null;
        currentWaypointIndex = 0;
    }
}