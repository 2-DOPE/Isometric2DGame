using System.Collections;
using UnityEngine;

/// <summary>
/// Defines the core behaviour of a character.
/// Handles movement and animation while delegating
/// physics and animation implementation to components.
/// </summary>
[RequireComponent(typeof(MovementComponent))]
[RequireComponent(typeof(CharacterStats))]
public abstract class Character : MonoBehaviour
{
    [Header("Essential Components")]
    [SerializeField] private CharacterStats stats;
    [SerializeField] private MovementComponent movement;
<<<<<<< Updated upstream
    [SerializeField] private AimingComponent aiming;

    public CharacterStats Stats => stats;
    public MovementComponent Movement => movement;
    public AimingComponent Aiming => aiming;
=======
    [SerializeField] private CombatComponent combat;
    [SerializeField] private Transform RotationPivot;

    private AnimationComponent animator;

    private bool isMoving;

    public CharacterStatsComponent Stats => stats;
    public MovementComponent Movement => movement;

    public CombatComponent Combat => combat;
    public AnimationComponent AnimationComponent => animator;
>>>>>>> Stashed changes

    protected virtual void Awake()
    {
        stats = GetComponent<CharacterStats>();
        movement = GetComponent<MovementComponent>();
<<<<<<< Updated upstream
        aiming = GetComponent<AimingComponent>();
=======
        animator = GetComponentInChildren<AnimationComponent>();
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
    public virtual void SetTarget(CharacterTarget target)
    {
        // Forward target to the relevant component(s).
    }

    public virtual void ClearTarget()
    {
        // Clear current target.
    }

}
=======
    #endregion

    #region Attack

    public void Attack()
    {
        if (!combat.CanAttack())
            return;

        StopMoving();

        animator.SetComboIndex(combat.UpdateComboIndex());
        animator.SetIsAttacking(true);

        combat.AttackDamage(RotationPivot);

        if (attackRoutine != null)
            StopCoroutine(attackRoutine);

        attackRoutine = StartCoroutine(
            ResetAttackAfterAnimation(0.5f)
        );
    }

    private Coroutine attackRoutine;

    private IEnumerator ResetAttackAfterAnimation(float duration)
    {
        yield return new WaitForSeconds(duration);

        animator.SetIsAttacking(false);
        attackRoutine = null;
    }
    #endregion


    #region Animation

    public virtual void SetFacingDirection(Vector2 direction)
    {
        Quaternion PivotRot = direction switch
        {
            var d when d.y > 0 && Mathf.Abs(d.x) <= .5 => Quaternion.Euler(0,0, 90f),
            var d when d.y < 0 && Mathf.Abs(d.x) <= .5 => Quaternion.Euler(0, 0, -90f),
            var d when d.x < 0 => Quaternion.Euler(0, 0, 180f),
            var d when d.x > 0 => Quaternion.Euler(0, 0, 0f),
            _ => Quaternion.identity   // Default
        };
        
        RotationPivot.rotation = PivotRot;

        animator.SetFacingDirection(direction);
    }

    #endregion
}
>>>>>>> Stashed changes
