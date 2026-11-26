using UnityEngine;

public class CharacterStatsComponent : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    public float CurrentHealth { get; private set; }

    private void Awake()
    {
        currentHealth = maxHealth;
    }
    
    #region Health
    // Apply incoming damage.
    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;

        CurrentHealth = Mathf.Max(0, CurrentHealth);

        if (CurrentHealth <= 0)
            Die();
    }

    // Handle death.
    private void Die()
    {
        Debug.Log($"{name} died.");
        Destroy(gameObject);
    }
    #endregion
}