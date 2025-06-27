using UnityEngine;

public class BigfootHitState : State<Bigfoot>
{
    private float time = 0.0f;

    public BigfootHitState(Bigfoot bigfoot)
        : base(bigfoot) { }


    public override void OnEnter()
    {
        time = 0.0f;
        owner.Animator.SetTrigger("Hit");
    }

    public override void OnUpdate()
    {
        owner.SkinnedMeshRenderer.material.color = Color.Lerp(Color.white, Color.red, Mathf.Sin(time * 40));
        time += Time.deltaTime;
    }

    public override void OnExit()
    {
        owner.SkinnedMeshRenderer.material.color = Color.white;
    }
}
