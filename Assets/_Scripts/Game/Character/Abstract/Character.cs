using UnityEngine;

/// <summary>
/// this enum is for controller type check
/// </summary>
enum CharacterControllerType
{
    Player, // Classic or Custom Player Controller
    Agent   // Type of AI Controller
}


/// <summary>
/// This Class is the Definition of what a character is in this game.
/// it holds Core parameters for a character object, and those must be set and respected for any class of this type.
/// Character is Always possessed by a Controller , either player or AI controller (the later can be Mob or passive NPC to be defined adherently)
/// </summary>
[RequireComponent(typeof(MovementComponent))]
[RequireComponent(typeof(CharacterStats))]
public abstract class Character : MonoBehaviour
{
    [Header("Essential Components")]
    [SerializeField] private CharacterStats stats;
    [SerializeField] private MovementComponent movement;
    [SerializeField] private AimingComponent aiming;

    public CharacterStats Stats => stats;
    public MovementComponent Movement => movement;
    public AimingComponent Aiming => aiming;

    protected virtual void Awake()
    {
        stats = GetComponent<CharacterStats>();
        movement = GetComponent<MovementComponent>();
        aiming = GetComponent<AimingComponent>();
    }

    public virtual void Move(Vector2 direction)
    {
        Movement.SetMoveDirection(direction);
    }

    public virtual void StopMoving()
    {
        Movement.StopMovement();
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