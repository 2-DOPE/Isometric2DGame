
using UnityEngine;

public abstract class CharacterControllerBase : MonoBehaviour
{
    [Header("Possession")]
    [SerializeField] private Character startingCharacter;

    public Character PossessedCharacter { get; private set; }
    public bool IsPossessing => PossessedCharacter != null;

    protected virtual void Start()
    {
        InitializePossession();
    }

    protected virtual void InitializePossession()
    {
        // Inspector assignment has priority.
        Character character = startingCharacter;

        // Otherwise, search for a Player-tagged character.
        if (character == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
                character = playerObject.GetComponent<Character>();
        }

        if (character == null)
        {
            Debug.LogWarning(
                $"{GetType().Name}: No character assigned or found with the Player tag.",
                this
            );
            return;
        }

        Possess(character);
    }

    public virtual bool Possess(Character character)
    {
        if (character == null)
            return false;

        if (character == PossessedCharacter)
            return true;

        if (IsPossessing)
            Unpossess();

        PossessedCharacter = character;
        OnPossess(character);

        return true;
    }

    public virtual void Unpossess()
    {
        if (!IsPossessing)
            return;

        Character previous = PossessedCharacter;
        PossessedCharacter = null;

        OnUnpossess(previous);
    }

    protected virtual void OnPossess(Character character) { }

    protected virtual void OnUnpossess(Character character)
    {
        character.StopMoving();
    }

    protected virtual void OnDisable()
    {
        Unpossess();
    }
}
