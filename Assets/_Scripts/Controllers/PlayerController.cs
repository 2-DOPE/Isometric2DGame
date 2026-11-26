using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles player input and character possession.
/// Allows the player to control different characters without
/// hardcoding input actions into individual characters.
/// </summary>
public class PlayerController : CharacterControllerBase
{
    [Header("Possessed Character on Begin Play")]
    [SerializeField] Character playerCharacter;
    
    [Header("Input Actions")]
    [SerializeField, InspectorName("Move")] private InputActionReference moveAction;
    [SerializeField, InspectorName("Camera Look")] private InputActionReference lookAction;
    
    [Space(22f)]
    [Header("References")]
    [SerializeField] private Camera mainCamera;

    [InspectorName("Camera Pan Component"), Tooltip("Refrence the Camera Pan Component in here")] 
    [SerializeField] private CameraPanComponent cameraPan;
    
    [InspectorName("Camera reset time"), Tooltip("Time before camera recenters to player when using Mouse")] 
    [SerializeField,Min(0)] private float camResetTime = 5f; // in Seconds


    private void OnEnable()
    {
        moveAction.action.performed += OnMove;
        moveAction.action.canceled += OnMoveCanceled;
        aimAction.action.performed += OnAim;
    }

    private new void OnDisable()
    {
        base.OnDisable();
    }
        base.OnDisable();
    }

    private void Update()
    {
        if (!IsPossessing)
            return;

        // Read movement input continuously.
        Vector2 input = moveAction.action.ReadValue<Vector2>();

        if (input.sqrMagnitude > 0.001f)
        {
            PossessedCharacter.Move(input);
        }
        else
        {
            PossessedCharacter.StopMoving();
        }

        //float ATKInput = AttackAction.action.ReadValue<float>();

        if (AttackAction.action.WasPerformedThisFrame())
        {
            PossessedCharacter.Attack();

        }
    }

    #region Possession

    protected override void OnPossess(Character character)
    {
        base.OnPossess(character);

        moveAction.action.Enable();
        aimAction.action.Enable();

    }

    protected override void OnUnpossess(Character character)
    {
        moveAction.action.Disable();
        aimAction.action.Disable();

        base.OnUnpossess(character);
    }
    #endregion

    #region Movement
    private void OnMove(InputAction.CallbackContext context)
    {
        //Debug.Log("MOVE INPUT RECEIVED: " + context.ReadValue<Vector2>());

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
    #endregion

    #region CameraPanComponent
    private void HandleCameraPan(InputAction.CallbackContext context)
    {
        if (context.control.device is Pointer)
        {
            Mathf.Clamp(context.control.device.magnitude, -1,1);
            cameraPan.CameraLookAt(context.ReadValue<Vector2>());
        }
        else //Reset immediatly
        {
            cameraPan.CameraLookAt(context.ReadValue<Vector2>());
        }

    }

    private void ResetCameraDelayed()
    {
        cameraPan.Reset();
    }

    #endregion

}
