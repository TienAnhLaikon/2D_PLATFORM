using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : PlayerStateBase
{
    public JumpState(PlayerScript player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine) { }
    public override void EnterState()
    {
        if (player.CanPerformAction(player.jumpStaminaCost))
        {
            Debug.Log("Enough Stamina");
            player.isJumping = true;
            //Debug.Log("Hello from jump state");
            player.animator.SetBool("Jump", player.isJumping);
            player.Jump();
            player.UseStamina(player.jumpStaminaCost);

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
        player.isJumping = false;

        //Debug.Log("Exit Jump State");
    }

    public override void FixedUpdate()
    {
        player.PlayerMove();

    }

    public override void Update()
    {
        // set the yVelocty in the animator
        player.animator.SetFloat("yVelocity", player.myRigidbody.velocity.y);
        if(player.myRigidbody.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            player.myRigidbody.velocity += Vector2.up * Physics2D.gravity.y * (player.lowJumpMultiplier - 1) * Time.deltaTime;

        }
        if (player.myRigidbody.velocity.y < -0.1f)
        {
            player.playerStateMachine.ChangeState(player.fallState);
        }
        if(player.isOnWall && !player.isGrounded && !player.isWallJumping)
        {
            //player.playerStateMachine.ChangeState(player.wallSlideState);
        }

            if (Input.GetMouseButtonDown(1))
            {
                player.playerStateMachine.ChangeState(player.dashState);

            }

        player.CheckMovementDirection();
    }

}
