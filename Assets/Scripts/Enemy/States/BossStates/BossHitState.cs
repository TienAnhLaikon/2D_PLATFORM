using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitState : BossStateBase
{
    public BossHitState(Boss boss, BossStateMachine bossStateMachine) : base(boss, bossStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        boss.isHitComplete = false;
        boss.animator.SetTrigger("GetHit");
        Debug.Log("Hello from boss hit state");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
        if (CheckIfAnimationComplete())
        {
            // chase
            if (CheckIfCanChase()) boss.bossStateMachine.ChangeState(boss.chaseState);

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
    private bool CheckIfAnimationComplete()
    {
        return boss.isHitComplete;
    }
}
