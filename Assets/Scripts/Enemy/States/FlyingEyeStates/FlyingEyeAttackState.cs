using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyeAttackState : EnemyStateBase
{
    private float attackDuration;
    public FlyingEyeAttackState(FlyingEnemy flyingEnemy, EnemyStateMachine enemyStateMachine) : base(flyingEnemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        //Debug.Log("Hello from flying enemy attack State");
        attackDuration = 0.25f;
        flyingEnemy.enemyRigidbody.velocity = Vector2.zero;
        flyingEnemy.isAttackComplete = false;

        flyingEnemy.animator.SetBool("hasTarget", flyingEnemy.isPlayerInAttackRange);
    }

    public override void ExitState()
    {
        base.ExitState();
        //Debug.Log("Exit attack State");
        flyingEnemy.animator.SetBool("hasTarget", false);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
        if (CheckIfAttackAnimationComplete())
        {
            if (CheckIfCanChase()) flyingEnemy.enemyStateMachine.ChangeState(flyingEnemy.flyingEyeChaseState);
            if (CheckIfCanPatrol()) flyingEnemy.enemyStateMachine.ChangeState(flyingEnemy.flyingEyePatrolState);
        }
        if (attackDuration >= 0f)
        {
            attackDuration -= Time.deltaTime;
            flyingEnemy.animator.SetFloat("attackDuration", attackDuration);
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
    private bool CheckIfCanPatrol()
    {
        if (!flyingEnemy.isPlayerInAttackRange && !flyingEnemy.isPlayerInDetectionRange)
        {
            return true;
        }
        return false;
    }
    private bool CheckIfAttackAnimationComplete()
    {
        return flyingEnemy.isAttackComplete;
    }
}
