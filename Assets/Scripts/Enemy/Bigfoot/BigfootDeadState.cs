
using System.Collections;
using UnityEngine;

public class BigfootDeadState : BigfootState
{
    public BigfootDeadState(Bigfoot bigfoot) 
        : base(bigfoot) { }

    public override void OnEnter()
    {
        bigfoot.Animator.SetBool("IsDead", true);
    }

    public override void OnExit()
    {
        bigfoot.Animator.SetBool("IsDead", false);
    }
}
