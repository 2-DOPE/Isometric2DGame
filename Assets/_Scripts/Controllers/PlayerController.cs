
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles player input and character possession.
/// Allows the player to control different characters without
/// hardcoding input actions into individual characters.
/// </summary>
public class PlayerController : CharacterControllerBase
{
<<<<<<< Updated upstream
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
=======
    [Header("Input Actions")]
    [SerializeField, InspectorName("Move")]
    private InputActionReference moveAction;

    [SerializeField, InspectorName("Camera Look")]
    private InputActionReference lookAction;

    [SerializeField, InspectorName("Attack")]
    private InputActionReference AttackAction;

    [Space(22f)]
    [Header("References")]
    [SerializeField] private Camera mainCamera;

    [InspectorName("Camera Pan Component")]
    [Tooltip("Reference the Camera Pan Component here")]
    [SerializeField] private CameraPanComponent cameraPan;

    [InspectorName("Camera reset time")]
    [Tooltip("Time before camera recenters to player when using Mouse")]
    [SerializeField, Min(0)]
    private float camResetTime = 5f;

    private void OnEnable()
    {
        lookAction.action.performed += HandleCameraPan;
        lookAction.action.canceled += ResetCamera;
>>>>>>> Stashed changes
    }

    private new void OnDisable()
    {
<<<<<<< Updated upstream
        base.OnDisable();
    }
=======
        lookAction.action.performed -= HandleCameraPan;
        lookAction.action.canceled -= ResetCamera;

        moveAction.action.Disable();
        lookAction.action.Disable();
        AttackAction.action.Disable();

        CancelInvoke(nameof(ResetCameraDelayed));

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
>>>>>>> Stashed changes

    protected override void OnPossess(Character character)
    {
        base.OnPossess(character);

        moveAction.action.Enable();
<<<<<<< Updated upstream
        aimAction.action.Enable();
=======
        lookAction.action.Enable();
        AttackAction.action.Enable();
>>>>>>> Stashed changes
    }

    protected override void OnUnpossess(Character character)
    {
        moveAction.action.Disable();
<<<<<<< Updated upstream
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
=======
        lookAction.action.Disable();
        AttackAction.action.Disable();

        // Prevent the previous character from continuing to move.
        character.StopMoving();

        base.OnUnpossess(character);
    }

    #endregion

    #region CameraPanComponent

    private void HandleCameraPan(InputAction.CallbackContext context)
    {
        if (context.control.device is Pointer)
        {
            cameraPan.CameraLookAt(context.ReadValue<Vector2>());
        }
        else
        {
            cameraPan.CameraLookAt(context.ReadValue<Vector2>());
        }
    }

    private void ResetCamera(InputAction.CallbackContext context)
    {
        if (context.control.device is Pointer)
        {
            CancelInvoke(nameof(ResetCameraDelayed));
            Invoke(nameof(ResetCameraDelayed), camResetTime);
        }
        else
        {
            CancelInvoke(nameof(ResetCameraDelayed));
            cameraPan.Reset();
        }
    }

    private void ResetCameraDelayed()
    {
        cameraPan.Reset();
    }

    #endregion
}
>>>>>>> Stashed changes
