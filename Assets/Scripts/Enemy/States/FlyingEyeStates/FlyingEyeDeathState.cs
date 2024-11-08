using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyeDeathState : EnemyStateBase
{
    public FlyingEyeDeathState(FlyingEnemy flyingEnemy, EnemyStateMachine enemyStateMachine) : base(flyingEnemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Hello from flying Death state");
        flyingEnemy.animator.SetBool("IsAlive", false);
        flyingEnemy.enemyRigidbody.gravityScale = 2f;
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
    }
}
