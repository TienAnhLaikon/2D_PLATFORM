using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FallState : PlayerStateBase
{
    public FallState(PlayerScript player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine) { }
    public override void EnterState()
    {
        player.isFalling = true;
        //Debug.Log("Hello from Fall State");
    }

    public override void ExitState()
    {
        player.isFalling =false;
        //Debug.Log("Exit fall State");
    }

    public override void FixedUpdate()
    {
        // xử lý rơi
        if (player.myRigidbody.velocity.y < 0)
        {
            player.myRigidbody.velocity += Vector2.up * Physics2D.gravity.y * (player.fallMultiplier - 1) * Time.deltaTime;
        }

        player.PlayerMove();
    }

    public override void Update()
    {

        // set the yVelocty in the animator
        player.animator.SetFloat("yVelocity", player.myRigidbody.velocity.y);
        if(player.myRigidbody.velocity.y <= 0.01f && player.isGrounded)
        {
            player.playerStateMachine.ChangeState(player.idleState);
        }
        if(player.isFalling && !player.isGrounded && player.isOnWall)
        {
            player.playerStateMachine.ChangeState(player.wallSlideState);
        }

            if (Input.GetMouseButtonDown(1))
            {
                player.playerStateMachine.ChangeState(player.dashState);

            }
        
        player.CheckMovementDirection();
    }
}
