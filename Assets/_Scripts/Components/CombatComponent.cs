using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class CombatComponent : MonoBehaviour
{
    [Header("Combat Settings")]
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private int MaxComboIndex = 1;

    protected int currentComboIndex = 0;

    private float lastAttackTime;

    // Check whether another attack can be performed.
    public bool CanAttack()
    {
        return Time.time >= lastAttackTime + attackCooldown;
    }

    // Perform an attack against the target.
    public void AttackDamage(Transform TracePoint)
    {
        if (TracePoint == null)
            return;

        Collider2D[] HitTarget = Physics2D.OverlapCircleAll(TracePoint.position, attackRange);

        foreach (Collider2D hitTarget in HitTarget)
        {
            CharacterStatsComponent DamagedChar = hitTarget.GetComponent<CharacterStatsComponent>();

            if (DamagedChar != null)
            {
                DamagedChar.TakeDamage(attackDamage);

                Debug.Log($"Attacked {hitTarget.name} for {attackDamage} damage.");
            }
        }
    }

    public int UpdateComboIndex()
    {
        currentComboIndex = (currentComboIndex + 1) % (MaxComboIndex + 1);

        lastAttackTime = Time.time;

        Debug.Log($"Current Combo Index {currentComboIndex}");

        return currentComboIndex;
    }

}