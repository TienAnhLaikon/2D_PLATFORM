using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollState : PlayerStateBase
{
    private float rollTimer; // thoi gian roll
    public RollState(PlayerScript player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine) { }
    public override void EnterState()
    {
        if (player.CanPerformAction(player.rollStaminaCost))
        {
            Debug.Log("Hello from Roll State");
            player.isRolling = true;
            player.canRoll = false;
            rollTimer = player.rollDuration;
            player.Roll();
            player.animator.SetTrigger("Roll");
            player.UseStamina(player.rollStaminaCost);
        }
        else
        {
            // khong du stamina de jump
            Debug.Log("Not enough stamina");
            player.playerStateMachine.ChangeState(player.idleState);
        }

    }

    public override void ExitState()
    {
        Debug.Log("Exit Roll State");
        player.isRolling = false;
        player.canRoll = true;
    }

    public override void FixedUpdate()
    {
    }

    public override void Update()
    {
        AnimatorStateInfo stateInfo = player.animator.GetCurrentAnimatorStateInfo(0);
        // kiem tra animatio ket thuc chua
        if(stateInfo.IsName("Roll")&& stateInfo.normalizedTime >= 1.0f)
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
