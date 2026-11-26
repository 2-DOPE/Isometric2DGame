using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationComponent : CharacterComponent
{

    [InspectorName("Character Animator"), Tooltip ("the Animator that handles this character's animations.")]
    [SerializeField] private Animator Animator;
    private Vector2 AnimDir;
    

    public void SetFacingDirection(Vector2 direction)
    {
        AnimDir = direction switch
        {
            var d when d.y > 0 && Mathf.Abs(d.x) <= .5=> Vector2.up,
            var d when d.y < 0 && Mathf.Abs(d.x) <= .5=> Vector2.down,
            var d when d.x < 0  => Vector2.left,
            var d when d.x > 0  => Vector2.right,
            _ => Vector2.zero   // Default
        };

        Animator.SetFloat("Dir_X", AnimDir.x);
        Animator.SetFloat("Dir_Y", AnimDir.y);
    }
    public void SetIsMoving(bool isMoving) 
    {
        Animator.SetBool("isMoving", isMoving);
    }
    public void SetIsAttacking(bool isAttacking)
    {
        Animator.SetBool("isAttacking", isAttacking);
    }

    public void ResetAttack()
    {
        Animator.SetBool("isAttacking", false);
    }

    public void TriggerCharacterHit(Collider2D collider2D)
    {
        Animator.SetTrigger("Hit");
    }

    public void SetComboIndex(int comboIndex) 
    {
        Animator.SetInteger("ComboIndex",comboIndex);

    }

}