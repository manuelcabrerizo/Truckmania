using UnityEngine;

public class BigfootHitState : State<Bigfoot>
{
    private float time = 0.0f;
    private float colorDec = 0.0f;

    public BigfootHitState(Bigfoot bigfoot)
        : base(bigfoot)
    {
        int lifeCount = bigfoot.GetMaxLife();
        colorDec = 1.0f / (float)lifeCount;
    }

    public override void OnEnter()
    {
        time = 0.0f;
        owner.Animator.SetTrigger("Hit");
        GameEventManager.Instance.TriggerEvent(PlayAudioClip3DEvent.GetEvent(owner.Clips.mounsterHit, owner.transform.position, 100, 400));
    }

    public override void OnExit()
    {
        Color color = owner.Color;
        color.g -= colorDec;
        color.b -= colorDec;
        owner.Color = color;
        owner.SkinnedMeshRenderer.material.SetColor("_Color", owner.Color);
        owner.SkinnedMeshRenderer.material.SetColor("_Tint", Color.black);
    }

    public override void OnUpdate()
    {
        owner.SkinnedMeshRenderer.material.SetColor("_Tint", Color.Lerp(Color.black, Color.green, Mathf.Sin(time * 40)));
        if (time >= 4.0f)
        {
            owner.SetIdleState();
        }
        time += Time.deltaTime;
    }
}
