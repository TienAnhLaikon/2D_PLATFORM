using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyStateBase
{
    private float animationSpeedMultiplier = 1.5f; // Tốc độ animation khi đuổi theo

    public EnemyChaseState(Enemy enemy, EnemyStateMachine enemyStateMachine, Transform player)
        : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        //Debug.Log("Hello from chase state");
        enemy.animator.speed = animationSpeedMultiplier; // tăng tốc độ animation lên
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.animator.speed = 1f;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        enemy.Chase();
    }

    public override void Update()
    {
        base.Update();
        enemy.animator.SetFloat("speed", Mathf.Abs(enemy.moveDirection.x));
        enemy.CheckMovementDirection();

            if (CheckIfCanIdle()) enemy.enemyStateMachine.ChangeState(enemy.idleState);
            if (CheckIfCanAttack()) enemy.enemyStateMachine.ChangeState(enemy.attackState);
        
        }
    private bool CheckIfCanIdle()
    {
        if (!enemy.isPlayerInDetectionRange)
        {
            return true;
        }
        return false;
    }
    private bool CheckIfCanAttack()
    {
        if (enemy.isPlayerInAttackRange)
        {
            return true;
        }
        return false;
    }
}
