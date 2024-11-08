using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyePatrolState : EnemyStateBase
{
    public FlyingEyePatrolState(FlyingEnemy flyingEnemy, EnemyStateMachine enemyStateMachine) : base(flyingEnemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        //Debug.Log("Hello form flying eye patrol state");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        flyingEnemy.Flight();

    }

    public override void Update()
    {
        base.Update();
        if (CheckIfCanChase()) flyingEnemy.enemyStateMachine.ChangeState(flyingEnemy.flyingEyeChaseState);
    }
    private bool CheckIfCanChase()
    {
        if (flyingEnemy.isPlayerInDetectionRange)
        {
            return true;
        }
        return false;
    }
}
