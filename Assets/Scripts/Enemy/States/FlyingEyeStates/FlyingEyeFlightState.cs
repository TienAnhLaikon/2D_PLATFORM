using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyeFlightState : EnemyStateBase
{
    public FlyingEyeFlightState(FlyingEnemy flyingEnemy, EnemyStateMachine enemyStateMachine) : base(flyingEnemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        
        base.EnterState();
        Debug.Log("Hello from flying eye flight state");
        
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
