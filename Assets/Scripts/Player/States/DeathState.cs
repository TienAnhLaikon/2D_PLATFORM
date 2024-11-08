using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : PlayerStateBase
{
    public DeathState(PlayerScript player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        //Debug.Log("Hello from death state");
        player.animator.SetBool("IsAlive", false);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
    }
}
