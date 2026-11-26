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
[RequireComponent(typeof(CharacterStatsComponent))]
public abstract class Character : MonoBehaviour
{
    //for Debug purposes only
    [Header("Essential Components")]
    [SerializeField] private CharacterStatsComponent stats;
    [SerializeField] private MovementComponent movement;

 
    public CharacterStatsComponent Stats => stats;
    public MovementComponent Movement => movement;
    

    protected virtual void Awake()
    {
        stats = GetComponent<CharacterStatsComponent>();
        movement = GetComponent<MovementComponent>();
    }

    public virtual void Move(Vector2 direction)
    {
        Movement.SetMoveDirection(direction);
    }

    public virtual void StopMoving()
    {
        Movement.StopMovement();
    }

}