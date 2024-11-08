using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RunState : PlayerStateBase
{
    public RunState(PlayerScript player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)   {  }

    public override void EnterState()
    {
            //Debug.Log("Hello from Run State");
       

    }

    public override void ExitState()
    {
        //Debug.Log("Exit Run State");

    }

    public override void FixedUpdate()
    {
        player.PlayerMove();

    }

    public override void Update()
    {
        if(player.moveDirection.x == 0 && player.isGrounded && !player.isMoving)
        {
            player.playerStateMachine.ChangeState(player.idleState);
        }
        if (Input.GetKeyDown(KeyCode.Space) && player.isGrounded)
        {
            player.playerStateMachine.ChangeState(player.jumpState);
        }
        // fall
        if (player.myRigidbody.velocity.y < 0 && !player.isGrounded)
        {
            player.playerStateMachine.ChangeState(player.fallState);
        }
        // roll
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            player.playerStateMachine.ChangeState(player.rollState);
        }
        // slide
        if (Input.GetKeyDown(KeyCode.LeftControl) && player.isGrounded)
        {
            player.playerStateMachine.ChangeState(player.slideState);
        }
        if (Input.GetMouseButtonDown(1))
        {
            player.playerStateMachine.ChangeState(player.dashState);

        }

                // attack
        if (Input.GetMouseButtonDown(0))
        {
            player.playerStateMachine.ChangeState(player.attackState);
        }
        player.CheckMovementDirection();
        if (CheckIfCanHeal()) player.playerStateMachine.ChangeState(player.healState);
    }
    private bool CheckIfCanHeal()
    {
        if (Input.GetKeyDown(KeyCode.Q) && player.estucFlasks > 0)
        {
            return true;
        }
        return false;
    }
}
