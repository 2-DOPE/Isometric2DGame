
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementComponent : MonoBehaviour, IMoveable
{
    [Header("References")]
    [SerializeField] private new Rigidbody2D rigidbody2D;

    [Header("Movement Settings")]
    // Min(0f) prevents the speed value from being set below zero in the Inspector.
    [SerializeField, Min(0f)] protected float speed = 5f;

    private Vector2 moveDirection;

    // Exposes the current movement direction through a read-only property.
    // Other components can read this value without directly modifying the internal field.
    public Vector2 MoveDirection => moveDirection;

    public float MoveSpeed => speed;

    protected virtual void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Clamps the movement direction's magnitude to a maximum of 1.
    /// This prevents diagonal movement from being faster than horizontal or vertical movement.
    /// </summary>
    /// <param name="direction">The movement direction received from the controller.</param>
    public void SetMoveDirection(Vector2 direction)
    {
        Debug.Log("MOVEMENT RECEIVED: " + direction);

        moveDirection = Vector2.ClampMagnitude(direction, 1f);
    }

    /// <summary>
    /// Resets the movement direction to zero, stopping movement.
    /// This can also prevent residual movement values from persisting,
    /// for example when movement input is interpolated.
    /// </summary>
    public void StopMovement()
    {
        moveDirection = Vector2.zero;
    }

   protected virtual void FixedUpdate()
{
    rigidbody2D.linearVelocity = moveDirection * speed;

    //Debug.Log($"PHYSICS VELOCITY: {rigidbody2D.linearVelocity}");
}
}
