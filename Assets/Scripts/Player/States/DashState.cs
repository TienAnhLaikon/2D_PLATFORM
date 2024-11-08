using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class DashState : PlayerStateBase
{
    private Coroutine dashCoroutine;

    public DashState(PlayerScript player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        if (player.CanPerformAction(player.dashStaminaCost))
        {

            // khi đã vào đây rồi là mặc định sẽ dash 1 lần nên ta không cho dash lần nữa 
            player.isDashing = true;
            //Debug.Log("hello from dash State");
            player.animator.SetBool("IsDashing", true);
            player.UseStamina(player.dashStaminaCost);
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
        player.isDashing = false;
        player.animator.SetBool("IsDashing", false);

        // Nếu coroutine đang chạy, dừng nó khi thoát khỏi DashState
        if (dashCoroutine != null)
        {
            player.StopCoroutine(dashCoroutine);
            dashCoroutine = null;
        }
        //Debug.Log("Exit from dash State");
    }

    public override void FixedUpdate()
    {
        if (!player.hasDashed || player.isGrounded)
        {
            Dash();
        }

    }

    public override void Update()
    {

        if (player.isDashing && player.isOnWall)
        {
            playerStateMachine.ChangeState(player.wallSlideState);

        }

    }
    public void Dash()
    {
        float dashDirection = player.isFacingRight ? 1f : -1f;
        player.myRigidbody.velocity = new Vector2(player.dashForce * dashDirection, 0f);
        player.isDashing = true;
        // Khởi chạy coroutine StopDash nếu chưa chạy
        // Đánh dấu đã dash trên không
        if (!player.isGrounded)
        {
            player.hasDashed = true;
        }
        if (dashCoroutine == null)
        {
            dashCoroutine = player.StartCoroutine(StopDash());
        }
    }
    IEnumerator StopDash()
    {
        yield return new WaitForSeconds(player.dashTime);
        player.isDashing = false;
        if (!player.isDashing && player.isGrounded)
        {
            player.myRigidbody.velocity = new Vector2(0f, player.myRigidbody.velocity.y);
            playerStateMachine.ChangeState(player.idleState);
            //Debug.Log("Idle");
        }
        if(!player.isDashing && !player.isGrounded)
        {
            player.myRigidbody.velocity = new Vector2(0f, player.myRigidbody.velocity.y);
            playerStateMachine.ChangeState(player.fallState);
            //Debug.Log("Fall");
        }
        //if (!player.isDashing && !player.isOnWall)
        //{
        //    player.playerStateMachine.ChangeState(player.runState);

        //}
        dashCoroutine = null;  // Reset biến coroutine để có thể khởi chạy lại

       // Debug.Log("Stop Dash");
    }
}
