using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyStateBase
{
    private float attackDuration;
    private bool isAttackStarted = false; // Biến này để kiểm soát việc bắt đầu tấn công chỉ xảy ra 1 lần

    public EnemyAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        attackDuration = enemy.attackDuration;
        //Debug.Log("Hello from enemy attack State");
        enemy.animator.SetFloat("speed", 0f);
        enemy.isAttackComplete = false;
        enemy.enemyRigidbody.velocity = Vector3.zero;   
        //enemy.animator.SetBool("hasTarget", enemy.isPlayerInAttackRange);
        isAttackStarted = false; // Đặt lại biến cờ khi vào state mới

    }

    public override void ExitState()
    {

        base.ExitState();
       // Debug.Log("Exit From enemy Attack state");
        enemy.animator.SetBool("hasTarget", false);

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
        // kiem tra animatio ket thuc chua

        if(attackDuration >= 0f)
        {
            attackDuration -= Time.deltaTime;
            enemy.animator.SetFloat("attackDuration",attackDuration);
        }
        else if(!isAttackStarted && attackDuration <= 0f)
        {
            isAttackStarted = true; // Đánh dấu là đã bắt đầu tấn công
            enemy.animator.SetBool("hasTarget",true); // Bắt đầu animation đánh
        }
        if (CheckIfAttackAnimationComplete())
        {
            if (CheckIfCanChase()) enemy.enemyStateMachine.ChangeState(enemy.chaseState);
            if (CheckIfCanIdle()) enemy.enemyStateMachine.ChangeState(enemy.idleState);
        }

    }
    private bool CheckIfCanChase()
    {
        if(enemy.isPlayerInDetectionRange)
        {
            return true;
        }
        return false;
    }
    private bool CheckIfCanIdle()
    {
        if(!enemy.isPlayerInAttackRange && !enemy.isPlayerInDetectionRange)
        {
            return true;
        }
        return false;
    }
    private bool CheckIfAttackAnimationComplete()
    {
        return enemy.isAttackComplete;
    }
}
