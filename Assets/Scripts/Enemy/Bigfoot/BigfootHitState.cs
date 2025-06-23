using UnityEngine;

public class BigfootHitState : BigfootState
{
    private float time = 0.0f;

    public BigfootHitState(Bigfoot bigfoot)
        : base(bigfoot) { }


    public override void OnEnter()
    {
        time = 0.0f;
        bigfoot.Animator.SetTrigger("Hit");
    }

    public override void OnUpdate()
    {
        bigfoot.SkinnedMeshRenderer.material.color = Color.Lerp(Color.white, Color.red, Mathf.Sin(time * 40));
        time += Time.deltaTime;
    }

    public override void OnExit()
    {
        bigfoot.SkinnedMeshRenderer.material.color = Color.white;
    }
}
