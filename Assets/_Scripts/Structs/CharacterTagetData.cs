using UnityEngine;

public struct CharacterTarget
{
    public Vector2 Position { get; private set; }
    public Transform Entity { get; private set; }

    public bool HasEntity => Entity != null;

    public CharacterTarget(Vector2 position)
    {
        Position = position;
        Entity = null;
    }

    public CharacterTarget(Transform entity)
    {
        Entity = entity;
        Position = entity != null
            ? (Vector2)entity.position
            : Vector2.zero;
    }
}