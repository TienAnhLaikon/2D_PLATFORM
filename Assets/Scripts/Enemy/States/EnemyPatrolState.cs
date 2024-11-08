using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemyPatrolState : EnemyStateBase
{
    public EnemyPatrolState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        //Debug.Log("Hello from Enemy Patrol State");

    }

    public override void ExitState()
    {
        //Debug.Log("Exit Patrol State");

        base.ExitState();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        enemy.Patrol();
    }

    public override void Update()
    {
        base.Update();
        enemy.animator.SetFloat("speed", Mathf.Abs(enemy.moveDirection.x));
        enemy.CheckMovementDirection();
        // kiểm tra có chạm đất nữa hay chạm tường nữa không
        if (CheckIfCanIdle())
        {
            enemy.enemyStateMachine.ChangeState(enemy.idleState);
         }
        // kiểm tra xem có thể tấn công player hay không?
        if (CheckIfCanAttack())
        {
            //enemy.enemyStateMachine.ChangeState(enemy.attackState);
        }
        if(CheckIfCanChase()) enemy.enemyStateMachine.ChangeState(enemy.chaseState);
    }
    private bool CheckIfCanIdle()
    {
        if (!enemy.isGrounded || enemy.isOnWall)
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
    private bool CheckIfCanChase()
    {
        if (enemy.isPlayerInDetectionRange)
        {
            return true;
        }
        return false;
    }


}
