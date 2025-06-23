
using System.Collections;
using UnityEngine;

public class BigfootDeadState : BigfootState
{
    private float time = 0.0f;
    private bool isDead = false;

    public BigfootDeadState(Bigfoot bigfoot) 
        : base(bigfoot) { }

    public override void OnEnter()
    {
        isDead = false;
        bigfoot.Animator.SetBool("IsDead", true);
    }

    public override void OnUpdate()
    {
        if (!isDead)
        {
            bigfoot.SkinnedMeshRenderer.material.color = Color.Lerp(Color.white, Color.red, Mathf.Sin(time * 40));
            time += Time.deltaTime;
        }
    }

    public override void OnExit()
    {
        bigfoot.SkinnedMeshRenderer.material.color = Color.white;
        bigfoot.Animator.SetBool("IsDead", false);
    }
    public void SetDead()
    {
        isDead = true;
        bigfoot.SkinnedMeshRenderer.material.color = Color.white;

    }
}
