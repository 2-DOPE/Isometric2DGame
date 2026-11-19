
using UnityEngine;

public interface IMoveable
{
    Vector2 MoveDirection { get; }

    float MoveSpeed { get; }

    void SetMoveDirection(Vector2 direction);

    void StopMovement();
}
