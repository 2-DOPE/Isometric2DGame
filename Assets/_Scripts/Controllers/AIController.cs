using UnityEngine;



public class AIController : CharacterControllerBase
{
    protected override void OnPossess(Character character)
    {
        base.OnPossess(character);

        // Initialize AI behaviour.
    }

    private void SetAIDestination(Transform target)
    {
        if (!IsPossessing || target == null)
            return;

        PossessedCharacter.SetTarget(
            new CharacterTarget(target)
        );
    }
}