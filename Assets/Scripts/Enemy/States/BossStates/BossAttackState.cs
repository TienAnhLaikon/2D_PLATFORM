using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackState : BossStateBase
{
    private float attackDuration;
    private bool isAttackStarted = false; // Biến này để kiểm soát việc bắt đầu tấn công chỉ xảy ra 1 lần
    public BossAttackState(Boss boss, BossStateMachine bossStateMachine) : base(boss, bossStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        attackDuration = boss.attackDuration;
        //Debug.Log("Hello from enemy attack State");
        boss.animator.SetFloat("speed", 0f);
        boss.isAttackComplete = false;
        boss.enemyRigidbody.velocity = Vector3.zero;
        //enemy.animator.SetBool("hasTarget", enemy.isPlayerInAttackRange);
        isAttackStarted = false; // Đặt lại biến cờ khi vào state mới
    }

    public override void ExitState()
    {
        base.ExitState();
        boss.animator.SetBool("hasTarget", false);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
        if (attackDuration >= 0f)
        {
            attackDuration -= Time.deltaTime;
            boss.animator.SetFloat("attackDuration", attackDuration);
        }
        else if (!isAttackStarted && attackDuration <= 0f)
        {
            isAttackStarted = true; // Đánh dấu là đã bắt đầu tấn công
            boss.animator.SetBool("hasTarget", true); // Bắt đầu animation đánh
        }
        if (CheckIfAttackAnimationComplete())
        {
            if (CheckIfCanChase()) boss.bossStateMachine.ChangeState(boss.chaseState);
            if (CheckIfCanIdle()) boss.bossStateMachine.ChangeState(boss.idleState);
        }
    }
    private bool CheckIfCanChase()
    {
        if (boss.isPlayerInDetectionRange)
        {
            return true;
        }
        return false;
    }
    private bool CheckIfCanIdle()
    {
        if (!boss.isPlayerInAttackRange && !boss.isPlayerInDetectionRange)
        {
            return true;
        }
        return false;
    }
    private bool CheckIfAttackAnimationComplete()
    {
        return boss.isAttackComplete;
    }
}
