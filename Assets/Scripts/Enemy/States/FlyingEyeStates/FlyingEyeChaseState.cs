using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class FlyingEyeChaseState : EnemyStateBase
{
    public FlyingEyeChaseState(FlyingEnemy flyingEnemy, EnemyStateMachine enemyStateMachine, Transform player) : base(flyingEnemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        //Debug.Log("Hello from flying enemy chase state");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        flyingEnemy.Chase();
    }

    public override void Update()
    {
        base.Update();
        flyingEnemy.CheckMovementDirection();
        if (CheckIfCanPatrol()) flyingEnemy.enemyStateMachine.ChangeState(flyingEnemy.flyingEyePatrolState);
        if (CheckIfCanAttack()) flyingEnemy.enemyStateMachine.ChangeState(flyingEnemy.flyingEyeAttackState);
    }
    private bool CheckIfCanPatrol()
    {
        if (!flyingEnemy.isPlayerInDetectionRange)
        {
            return true;
        }
        return false;
    }
    private bool CheckIfCanAttack()
    {
        if (flyingEnemy.isPlayerInAttackRange)
        {
            return true;
        }
        return false;
    }
}
