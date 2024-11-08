using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : PlayerStateBase
{
    private bool hasRegisteredComboClick; // Biến để kiểm tra nếu đã nhận lần nhấp chuột thứ 2

    public AttackState(PlayerScript player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine) { }
    public override void EnterState()
    {
        if (player.CanPerformAction(player.attackStaminaCost))
        {
            player.comboStep++;
            player.isAttacking = true;
            player.animator.SetTrigger("Attack");
            // Tắt di chuyển khi bắt đầu tấn công
            player.comboTimer = player.comboDuration;
            hasRegisteredComboClick = false; // Reset lại biến này mỗi khi vào attack state

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
        player.isAttacking = false;
        player.isKeepCombo = false; ;
        player.comboStep = 0;
        //Debug.Log("Exit attack State");

    }

    public override void FixedUpdate()
    {
        player.myRigidbody.velocity = Vector2.zero;
    }

    public override void Update()
    {
        if (player.CanPerformAction(player.attackStaminaCost))
        {
            // nếu player ấn nút tấn công trong lúc animation attack 1 đang chạy
            if (CheckIfCanComboAttackk() && !hasRegisteredComboClick)
            {
                player.animator.SetTrigger("Attack");

                player.isKeepCombo = true;
                hasRegisteredComboClick = true; // Đánh dấu rằng lần nhấp chuột đã được ghi nhận

            }
        }


    }
    private bool CheckIfCanComboAttackk()
    {
        if (Input.GetMouseButtonDown(0) && player.comboStep < player.maxComboStep)
        {
            return true;
        }
        return false;
    }
}
