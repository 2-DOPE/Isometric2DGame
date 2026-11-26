using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This the class by wich the player gets to control characters in the game.
/// it is mostly responsible for taking control over an actor without hard coding the input actions into the latter, and let's us swap charcters in game smoothly.
/// </summary>
public class PlayerController : CharacterControllerBase
{
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
        lookAction.action.performed += HandleCameraPan;
        lookAction.action.canceled += ResetCamera;
    }
    private void OnDisable()
    {
        //moveAction.action.performed -= OnMove;
        //moveAction.action.canceled -= OnMoveCanceled;
        //lookAction.action.performed -= HandleCameraPan;
        base.OnDisable();
    }
    #region Possession

    /// <summary>
    /// Event function that handles the logic when taking control over a character.
    /// 
    /// </summary>
    /// <param name="character"> The character to be possessed</param>
    protected override void OnPossess(Character character)
    {
        base.OnPossess(character);

        moveAction.action.Enable();
        lookAction.action.Enable();
    }
    /// <summary>
    /// Event function that handles the logic when taking control off a character.
    /// </summary>
    /// <param name="character"></param>
    protected override void OnUnpossess(Character character)
    {
        moveAction.action.Disable();
        lookAction.action.Disable();

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
    private void ResetCamera(InputAction.CallbackContext context)
    {
        // check if input comes from mouse
        if(context.control.device is Pointer)
        {
            // Delay for few seconds before resetting the camera
            Invoke(nameof(ResetCameraDelayed), camResetTime);
        }
        else //Reset immediatly
        {
            cameraPan.Reset();
        }
    }
    private void ResetCameraDelayed()
    {
        cameraPan.Reset();
    }
    #endregion

}