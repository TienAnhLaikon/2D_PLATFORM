using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChaseState : BossStateBase
{
    public BossChaseState(Boss boss, BossStateMachine bossStateMachine, Transform player) : base(boss, bossStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        // Debug.Log("Hello from boss Chase state");
        UIManager.Instance.bossHealth_Bar.Show();


    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        boss.Chase();
    }

    public override void Update()
    {
        base.Update();
        boss.animator.SetFloat("speed", Mathf.Abs(boss.moveDirection.x));
        boss.CheckMovementDirection();
        if (CheckIfCanIdle()) boss.bossStateMachine.ChangeState(boss.idleState);
        if (CheckIfCanAttack()) boss.bossStateMachine.ChangeState(boss.attackState);
    }
    private bool CheckIfCanIdle()
    {
        if (!boss.isPlayerInDetectionRange)
        {
            return true;
        }
        return false;
    }
    private bool CheckIfCanAttack()
    {
        if (boss.isPlayerInAttackRange)
        {
            return true;
        }
        return false;
    }
}
