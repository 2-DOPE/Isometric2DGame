using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This the class by wich the player gets to control characters in the game.
/// it is mostly responsible for taking control over an actor without hard coding the input actions into the latter, and let's us swap charcters in game smoothly.
/// </summary>
public class PlayerController : CharacterControllerBase
{
    [Header("Possessed Character on Begin Play")]
    [SerializeField] Character playerCharacter;
    
    [Header("Input Actions")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference aimAction;

    [Header("References")]
    [SerializeField] private Camera mainCamera;

    private void OnEnable()
    {
        moveAction.action.performed += OnMove;
        moveAction.action.canceled += OnMoveCanceled;
        aimAction.action.performed += OnAim;
    }
    private void OnDisable()
    {
        base.OnDisable();
    }

    /// <summary>
    /// Event function that handles the logic when taking control over a character.
    /// 
    /// </summary>
    /// <param name="character"> The character to be possessed</param>
    protected override void OnPossess(Character character)
    {
        base.OnPossess(character);

        moveAction.action.Enable();
        aimAction.action.Enable();
    }
    /// <summary>
    /// Event function that handles the logic when taking control off a character.
    /// </summary>
    /// <param name="character"></param>
    protected override void OnUnpossess(Character character)
    {
        moveAction.action.Disable();
        aimAction.action.Disable();

        base.OnUnpossess(character);
    }
    private void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log("MOVE INPUT RECEIVED: " + context.ReadValue<Vector2>());

        if (!IsPossessing)
        {
            Debug.LogWarning("PlayerController is NOT possessing a character.");
            return;
        }

        PossessedCharacter.Move(context.ReadValue<Vector2>());
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        if (!IsPossessing) return;

        PossessedCharacter.StopMoving();
    }

    private void OnAim(InputAction.CallbackContext context)
    {
        if (!IsPossessing || mainCamera == null)
            return;

        Vector2 screenPosition = context.ReadValue<Vector2>();

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(
            new Vector3(
                screenPosition.x,
                screenPosition.y,
                Mathf.Abs(mainCamera.transform.position.z)
            )
        );

        PossessedCharacter.SetTarget(
            new CharacterTarget(worldPosition)
        );
    }
}