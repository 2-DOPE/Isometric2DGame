
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
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
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
