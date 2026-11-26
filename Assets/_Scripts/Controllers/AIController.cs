using UnityEngine;

public class AIController : CharacterControllerBase
{
    [Header("AI State Machine")]
    [SerializeField] private AIStateMachine stateMachine;
    private PerceptionComponent perception;

    protected override void OnPossess(Character character)
    {
        base.OnPossess(character);

        if (stateMachine == null)
        {
            Debug.LogWarning("AIController has no StateMachine assigned.");
            return;
        }
        perception = character.GetComponent<PerceptionComponent>();
        stateMachine.Setup(this, character);
        stateMachine.Initialize();
    }

    private void Update()
    {
        if (!IsPossessing)
            return;

        stateMachine?.Tick();

        perception?.UpdatePerception();
    }

    protected override void OnUnpossess(Character character)
    {
        stateMachine?.Shutdown();

        base.OnUnpossess(character);
    }
}