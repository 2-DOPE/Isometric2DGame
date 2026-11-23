
using UnityEngine;

public class CharacterStats : MonoBehaviour, IStatistics
{
    [Header("Health Settings")]
    [SerializeField, Min(1f)]
    private float maxHealth = 100f;
    private float currentHealth;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    public bool IsAlive => currentHealth > 0f;

    protected virtual void Awake()
    {
<<<<<<< Updated upstream
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
=======
        CurrentHealth = maxHealth;
    }
    #region Health
    // Apply incoming damage.
    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        // Call animator or 
        CurrentHealth = Mathf.Max(0, CurrentHealth);

        if (CurrentHealth <= 0)
            Die();
    }

    // Handle death.
    protected void Die()
>>>>>>> Stashed changes
    {
        if (!IsAlive || amount <= 0f)
            return;

        currentHealth = Mathf.Max(0f, currentHealth - amount);
    }

    public void Heal(float amount)
    {
        if (!IsAlive || amount <= 0f)
            return;

        currentHealth = Mathf.Min(
            maxHealth,
            currentHealth + amount
        );
    }
}
