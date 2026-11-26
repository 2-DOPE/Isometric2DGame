using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Distance : Stimulus
{
    [SerializeField] private LayerMask targetMask;

    private PerceptionComponent perception;

    private void Awake()
    {
        GetComponent<CircleCollider2D>().isTrigger = true;
        perception = GetComponent<PerceptionComponent>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & targetMask) == 0)
            return;

        GameObject actor = other.attachedRigidbody != null
            ? other.attachedRigidbody.gameObject
            : other.gameObject;

        perception?.RegisterActor(actor, this);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & targetMask) == 0)
            return;

        GameObject actor = other.attachedRigidbody != null
            ? other.attachedRigidbody.gameObject
            : other.gameObject;

        perception?.RemoveActor(actor, this);
    }

    public override void OnPerception(GameObject perceiver)
    {
        // Detection is handled by trigger callbacks.
    }
}