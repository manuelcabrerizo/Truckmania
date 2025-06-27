using UnityEngine;

public class BigfootDeadState : State<Bigfoot>
{
    private float time = 0.0f;
    private bool isDead = false;

    public BigfootDeadState(Bigfoot bigfoot) 
        : base(bigfoot) { }

    public override void OnEnter()
    {
        isDead = false;
        owner.Animator.SetBool("IsDead", true);
    }

    public override void OnUpdate()
    {
        if (!isDead)
        {
            owner.SkinnedMeshRenderer.material.color = Color.Lerp(Color.white, Color.red, Mathf.Sin(time * 40));
            time += Time.deltaTime;
        }
    }

    public override void OnExit()
    {
        owner.SkinnedMeshRenderer.material.color = Color.white;
        owner.Animator.SetBool("IsDead", false);
    }
    public void SetDead()
    {
        isDead = true;
        owner.SkinnedMeshRenderer.material.color = Color.white;

    }
}
