using System;
using System.Collections;
using UnityEngine;

public class PlayerFlipState : State<Player>
{
    private float sign = 1.0f;
    public PlayerFlipState(Player owner, Func<bool> enterCondition, Func<bool> exitCondition)
    : base(owner, enterCondition, exitCondition) { }

    public override void OnEnter()
    {
        PlayerData data = owner.Data;
        sign = Mathf.Sign(owner.Data.flip);
        owner.StartCoroutine(PlayFlipAnimation(2.0f));
    }

    public override void OnExit() 
    {
        owner.StopAllCoroutines();
        owner.Data.trickDone = false;
    }

    public override void OnFixedUpdate()
    {
        PlayerData data = owner.Data;
        data.body.AddTorque(Vector3.up * (data.steer * data.playerData.airRotVelocity), ForceMode.Acceleration);
    }

    IEnumerator PlayFlipAnimation(float duration)
    {
        float time = 0;
        while (time < duration && owner.Data.trickDone == false)
        {
            Debug.Log("Flip");
            time += Time.deltaTime;
            float inc = 360.0f / (duration * Application.targetFrameRate);
            owner.transform.Rotate(Vector3.right * sign, inc);
            yield return new WaitForEndOfFrame();
        }
        owner.Data.trickDone = true;
    }
}