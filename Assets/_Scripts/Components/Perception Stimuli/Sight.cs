using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sight : Stimulus
{
    [Header("Sight Settings")]
    [SerializeField] private float viewDistance = 20f;
    [SerializeField] private float viewAngle = 90f;

    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;

    private PerceptionComponent perception;
    private void Awake()
    {
        perception = GetComponent<PerceptionComponent>();
    }
    public override void OnPerception(GameObject perceiver)
    {
        PerceptionComponent perception =
            perceiver.GetComponent<PerceptionComponent>();

        if (perception == null)
            return;

        Collider2D[] targets = Physics2D.OverlapCircleAll(
            perceiver.transform.position,
            viewDistance,
            targetMask
        );

        // Track actors visible during this update.
        HashSet<GameObject> visibleActors = new();

        foreach (Collider2D target in targets)
        {
            GameObject actor = target.attachedRigidbody != null
                ? target.attachedRigidbody.gameObject
                : target.gameObject;

            Vector2 direction =
                (Vector2)(target.transform.position - perceiver.transform.position);

            float distance = direction.magnitude;
            direction.Normalize();

            // Check field of view.
            float angle = Vector2.Angle(
                perceiver.transform.right,
                direction
            );

            if (angle > viewAngle * 0.5f)
                continue;

            // Check for obstacles.
            RaycastHit2D hit = Physics2D.Raycast(
                perceiver.transform.position,
                direction,
                distance,
                obstacleMask
            );

            if (hit.collider != null)
                continue;

            // Register visible actor.
            visibleActors.Add(actor);
            perception.RegisterActor(actor, this);
        }

        // Remove actors no longer visible to Sight.
        foreach (GameObject actor in perception.GetPerceivedActors())
        {
            if (!visibleActors.Contains(actor))
                perception.RemoveActor(actor, this);
        }
    }
}