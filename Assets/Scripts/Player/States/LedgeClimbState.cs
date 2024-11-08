using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeClimbState : PlayerStateBase
{
    public LedgeClimbState(PlayerScript player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("hello from ledge climb state");
        player.animator.SetBool("CanClimbLedge", player.canClimbLedge);
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
