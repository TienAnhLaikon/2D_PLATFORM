using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideState : PlayerStateBase
{
    public SlideState(PlayerScript player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Hello from Slide State");
        player.animator.SetTrigger("Slide");
        player.Slide();
    }

    public override void ExitState()
    {
        Debug.Log("Exit from Slide State");

    }

    public override void FixedUpdate()
    {

    }

    public override void Update()
    {
        AnimatorStateInfo stateInfo = player.animator.GetCurrentAnimatorStateInfo(0);
        // kiem tra animatio ket thuc chua
        if (stateInfo.IsName("Slide") && stateInfo.normalizedTime >= 1.0f)
        {
            // idle
            if (player.moveDirection.x == 0 && !player.isMoving)
            {
                player.playerStateMachine.ChangeState(player.idleState);
            }
            if (player.moveDirection.x != 0 && player.isMoving)
            {
                player.playerStateMachine.ChangeState(player.runState);
            }
        }
    }
}
