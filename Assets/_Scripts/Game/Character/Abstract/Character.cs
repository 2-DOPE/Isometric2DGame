using System.Collections;
using UnityEngine;

/// <summary>
/// Defines the core behaviour of a character.
/// Handles movement and animation while delegating
/// physics and animation implementation to components.
/// </summary>
[RequireComponent(typeof(MovementComponent))]
[RequireComponent(typeof(CharacterStatsComponent))]
public abstract class Character : MonoBehaviour
{
    //for Debug purposes only
    [Header("Essential Components")]
    [SerializeField] private CharacterStatsComponent stats;
    [SerializeField] private MovementComponent movement;
    [SerializeField] private AimingComponent aiming;

 
    public CharacterStatsComponent Stats => stats;
    public MovementComponent Movement => movement;
    public AimingComponent Aiming => aiming;

    protected virtual void Awake()
    {
        stats = GetComponent<CharacterStatsComponent>();
        movement = GetComponent<MovementComponent>();
        aiming = GetComponent<AimingComponent>();
    }

    #region Movement

    public virtual void Move(Vector2 direction)
    {
        // Debug.Log($"MOVE CALLED: {Time.frameCount}");
        
        // Clamp input to prevent faster diagonal movement.
        direction = Vector2.ClampMagnitude(direction, 1f);

        // Update movement direction.
        Movement.SetMoveDirection(direction);

        // Update facing direction only when meaningful input exists.
        if (direction.sqrMagnitude > 0.001f)
        {
            SetFacingDirection(direction);

            // Only update Animator when movement state changes.
            if (!isMoving)
            {
                isMoving = true;
                animator.SetIsMoving(true);
            }
        }
        else
        {
            StopMoving();
        }
    }

    public virtual void StopMoving()
    {
        // Debug.Log($"STOP CALLED: {Time.frameCount}");
        Movement.StopMovement();

        // Avoid redundant Animator updates.
        if (isMoving)
        {
            isMoving = false;
            animator.SetIsMoving(false);
        }
    }

    public virtual void SetTarget(CharacterTarget target)
    {
        // Forward target to the relevant component(s).
    }

    public virtual void ClearTarget()
    {
        // Clear current target.
    }

}
