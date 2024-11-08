using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HitState : PlayerStateBase
{
    public HitState(PlayerScript player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine) { }
    public override void EnterState()
    {
        //.Log("Hello from Hit State");
        player.animator.SetTrigger("GetHit");
        //KnockBack();
    }

    public override void ExitState()
    {
       // Debug.Log("Exit Hit State");
    }

    public override void FixedUpdate()
    {
    }

    public override void Update()
    {
        AnimatorStateInfo stateInfo = player.animator.GetCurrentAnimatorStateInfo(0);
        // kiem tra animatio ket thuc chua
        if (stateInfo.IsName("Hit") && stateInfo.normalizedTime >= 1.0f)
        {
            // idle
            if (CheckIfCanIdle())
            {
                player.playerStateMachine.ChangeState(player.idleState);
            }
            if (CheckIfCanFall())
            {
                player.playerStateMachine.ChangeState(player.fallState);
            }
        }
    }

    private bool CheckIfCanIdle()
    {
        if (player.isGrounded)
        {
            return true;
        }
        return false;
    }
    private bool CheckIfCanFall()
    {
        if (!player.isGrounded)
        {
            return true;
        }
        return false;
    }
}
