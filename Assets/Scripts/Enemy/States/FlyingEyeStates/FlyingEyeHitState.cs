using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyeHitState : EnemyStateBase
{
    public FlyingEyeHitState(FlyingEnemy flyingEnemy, EnemyStateMachine enemyStateMachine) : base(flyingEnemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Hello from flying enemy hit state");
        flyingEnemy.isHitComplete = false;
        flyingEnemy.animator.SetTrigger("GetHit");
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("exit from enemy hit state");

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
        if (CheckIfAnimationComplete()) {
            if (CheckIfCanChase()) flyingEnemy.enemyStateMachine.ChangeState(flyingEnemy.flyingEyeChaseState);

        }
    }
    private bool CheckIfCanChase()
    {
        if (flyingEnemy.isPlayerInDetectionRange)
        {
            return true;
        }
        return false;
    }
    private bool CheckIfAnimationComplete()
    {
        return flyingEnemy.isHitComplete;
    }
}
