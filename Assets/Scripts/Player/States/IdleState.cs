using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class IdleState : PlayerStateBase
{
    public IdleState(PlayerScript player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine){}

    public override void EnterState()
    {
        // player.myRigidbody.velocity = new Vector2(0f, player.myRigidbody.velocity.y
        //Debug.Log("Hello from Idle State");
        player.animator.SetFloat("yVelocity", 0f);

    }

    public override void ExitState()
    {
        //Debug.Log("Exit State");
    }

    public override void FixedUpdate()
    {
        // Không có xử lý vật lý đặc biệt trong trạng thái Idle, nên để trống

    }

    public override void Update()
    {
        if(CheckIfCanRun()) player.playerStateMachine.ChangeState(player.runState);
        //Jump
        if (CheckIfCanJump()) player.playerStateMachine.ChangeState(player.jumpState);

        // fall
        if (CheckIfCanFall()) player.playerStateMachine.ChangeState(player.fallState);
        
        // attack
        if (CheckIfCanAttack()) player.playerStateMachine.ChangeState(player.attackState);
       
        // roll
        if (CheckIfCanRoll()) player.playerStateMachine.ChangeState(player.rollState);
        
        // slide
        if (CheckIfCanSlide())  player.playerStateMachine.ChangeState(player.slideState);
        
        // heal
        if (CheckIfCanHeal()) player.playerStateMachine.ChangeState(player.healState);

        if (CheckIfCanDash()) player.playerStateMachine.ChangeState(player.dashState);

        
        player.CheckMovementDirection();
    }
    private bool CheckIfCanRun()
    {
        // run
        if (player.moveDirection.x != 0)
        {
            return true;
        }
        return false;
    }
    private bool CheckIfCanJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && player.isGrounded)
        {
            return true;
            }
        return false;
    }
    private bool CheckIfCanFall()
    {
        if(player.myRigidbody.velocity.y < 0 && !player.isGrounded)
        {
            return true;
            
        }
        return false;
    }
    private bool CheckIfCanAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            return true;
        }
        return false;
    }
    private bool CheckIfCanRoll()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) && player.isGrounded)
        {
            return true;
        }
        return false;
    }
    private bool CheckIfCanSlide()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl) && player.isGrounded)
        {
            return true;
        }
        return false;
    }
    private bool CheckIfCanHeal()
    {
        if (Input.GetKeyDown(KeyCode.Q) && player.estucFlasks > 0)
        {
            return true;
        }
        return false;
    }
    private bool CheckIfCanDash()
    {
        if (Input.GetMouseButtonDown(1))
        {
            return true;
        }
        return false;
    }
}

