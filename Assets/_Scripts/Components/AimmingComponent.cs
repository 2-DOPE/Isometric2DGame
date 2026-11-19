using UnityEngine;

public class AimingComponent : CharacterComponent
{
    [Header("References")]
    [SerializeField] private Transform visualRoot;
    [SerializeField] private Camera mainCamera;

    public void AimAt(Vector2 worldPosition)
    {
        if (visualRoot == null) return;

        Vector2 direction =
            worldPosition - (Vector2)visualRoot.position;

        if (direction.sqrMagnitude < 0.001f) return;

        float angle = Mathf.Atan2(
            direction.y,
            direction.x
        ) * Mathf.Rad2Deg;

        visualRoot.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}