
using UnityEngine;

/// <summary>
/// Base class for optional Character gameplay components.
/// Components deriving from this class must live
/// on the same GameObject as their Character.
/// </summary>
public abstract class CharacterComponent : MonoBehaviour
{
    protected Character Owner { get; private set; }

    protected virtual void Awake()
    {
        Owner = GetComponent<Character>();

        if (Owner == null)
        {
            Debug.LogError(
                $"{GetType().Name} requires a Character on the same GameObject.",
                this
            );

            enabled = false;
        }
    }
}
