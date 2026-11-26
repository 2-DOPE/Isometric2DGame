using UnityEngine;

public class CombatComponent : MonoBehaviour
{
    [Header("Combat Settings")]
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1f;

    private float lastAttackTime;

    // Check whether the target is within attack range.
    public bool IsInRange(GameObject target)
    {
        if (target == null)
            return false;

        return Vector2.Distance(
            transform.position,
            target.transform.position
        ) <= attackRange;
    }

    // Check whether another attack can be performed.
    public bool CanAttack()
    {
        return Time.time >= lastAttackTime + attackCooldown;
    }

    // Perform an attack against the target.
    public void Attack(GameObject target)
    {
        if (target == null || !IsInRange(target) || !CanAttack())
            return;

        lastAttackTime = Time.time;

        CharacterStatsComponent health = target.GetComponent<CharacterStatsComponent>();

        if (health != null)
            health.TakeDamage(attackDamage);

        Debug.Log($"Attacked {target.name} for {attackDamage} damage.");
    }
}