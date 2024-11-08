using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitState : EnemyStateBase
{
    public EnemyHitState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        //Debug.Log("Hello from Enemy Hit State");
        enemy.isHitComplete = false;
        enemy.animator.SetTrigger("GetHit");
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
            if (CheckIfCanChase()) enemy.enemyStateMachine.ChangeState(enemy.chaseState);

        }

    }
    private bool CheckIfCanChase()
    {
        if (enemy.isPlayerInDetectionRange)
        {
            return true;
        }
        return false;
    }
    private bool CheckIfCanAttack()
    {
        if(enemy.isPlayerInAttackRange && enemy.isPlayerInDetectionRange)
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
    private bool CheckIfAnimationComplete()
    {
        return enemy.isHitComplete;
    }
}
