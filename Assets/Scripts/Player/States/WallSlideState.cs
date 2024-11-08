using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlideState : PlayerStateBase
{
    public WallSlideState(PlayerScript player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine) { }

    private float wallSlideDelay = 0.5f;
    private float wallSlideTimer = 0f;
    public override void EnterState()
    {
        player.isWallSliding = true;
        //Debug.Log("Hello from Wall Slide state");

        player.animator.SetBool("IsWallSliding", player.isWallSliding);
        wallSlideTimer = 0;
    }

    public override void ExitState()
    {
        player.isWallSliding = false;
        player.animator.SetBool("IsWallSliding", player.isWallSliding);
        //Debug.Log("Exit Wall Slide state");
    }

    public override void FixedUpdate()
    {
        if (player.isWallSliding)
        {
            HandleWallSlide();
        }
    }

    public override void Update()
    {
        if(player.isGrounded && player.isWallSliding)
        {
            Debug.Log("Turn to idle");
            player.FlipDirection();
            player.playerStateMachine.ChangeState(player.idleState);
        }
        if(player.isWallSliding && player.isOnWall && Input.GetAxisRaw("Vertical") == -1)
        {
            player.FlipDirection();
            player.playerStateMachine.ChangeState(player.fallState);
        }
        if(player.isOnWall && player.isWallSliding && !player.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            player.playerStateMachine.ChangeState(player.wallJumpState);
            }

    }

    private void HandleWallSlide()
    {
        if (wallSlideTimer < wallSlideDelay)
        {
            wallSlideTimer += Time.deltaTime;
            //player.myRigidbody.velocity = Vector2.zero;
            //player.myRigidbody.gravityScale = 0;  // Vô hiệu hóa trọng lực
            player.myRigidbody.velocity = new Vector2(player.myRigidbody.velocity.x, 0);
            //Debug.Log("Not Slide");
        }
        else
        {
            //player.myRigidbody.gravityScale =1;  // Khôi phục trọng lực ban đầu
            //Debug.Log("Slide");
            float targetSlideSpeed = player.slowWallSlideSpeed;
            // Gọi hàm WallSlide của player với tốc độ trượt tương ứng
            player.WallSlide(targetSlideSpeed);
        }
    }
}
