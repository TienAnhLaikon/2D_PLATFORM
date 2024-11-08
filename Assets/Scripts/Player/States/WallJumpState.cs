using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WallJumpState : PlayerStateBase
{
    public WallJumpState(PlayerScript player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        player.isWallJumping = true;
        Debug.Log("Hello from wallJumpState");
        player.animator.SetBool("Jump", player.isWallJumping);
        WallJump();
    }

    public override void ExitState()
    {
        player.isWallJumping = false;
        player.animator.SetBool("Jump", player.isWallJumping);
        Debug.Log("Exit Wall jump state");
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        player.animator.SetFloat("yVelocity", player.myRigidbody.velocity.y);
        //if (player.myRigidbody.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        //{
        //    player.myRigidbody.velocity += Vector2.up * Physics2D.gravity.y * (player.lowJumpMultiplier - 1) * Time.deltaTime;

        //}
        if (player.myRigidbody.velocity.y < -0.1f)
        {
            player.playerStateMachine.ChangeState(player.fallState);
        }
        if (player.isOnWall && !player.isWallJumping)
        {
            player.playerStateMachine.ChangeState(player.wallSlideState);
        }
        //player.PlayerMove();
        player.CheckMovementDirection();
    }
    public void WallJump()
    {
        // Đảm bảo rằng hướng nhảy là ngược với hướng mà nhân vật đang đối mặt
        int wallDirection = player.isFacingRight ? -1 : 1;

        Vector2 force = new Vector2(wallDirection * player.wallJumpForce * player.wallJumpDirection.x, player.wallJumpForce * player.wallJumpDirection.y);
        player.myRigidbody.velocity = Vector2.zero;
        player.FlipDirection();
        player.myRigidbody.AddForce(force,ForceMode2D.Impulse);


    }
}
