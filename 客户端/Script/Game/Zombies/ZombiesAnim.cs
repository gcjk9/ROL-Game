using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiesAnim : ZombiesMotor
{
    public Animator animator;

    private float attackBlendFlag = 0.3f;

    public void AnimUpdate()
    {
        if (isDead)
            return;

        animator.SetFloat("speed", speed/speedMax);
        isFall = animator.GetBool("isFall");
    }
    public void AnimAttack()
    {
        Debug.Log("AnimAttack()");
        attackBlendFlag *= -1;
        float attackBlend = 0.5f + attackBlendFlag + Random.Range(-0.2f, 0.2f);
        animator.SetFloat("attackBlend", attackBlend);
        animator.CrossFadeInFixedTime("attack", 0.1f);
    }
    public void AnimDamage()
    {
        animator.SetBool("isDamage", true);
        animator.SetBool("isFall", isFall);
        animator.SetBool("isDead", isDead);
    }
}
