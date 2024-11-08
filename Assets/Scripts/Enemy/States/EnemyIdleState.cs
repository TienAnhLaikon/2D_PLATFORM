using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemyIdleState : EnemyStateBase
{
    private float idleTimer = 0f;
    private float idleDuration = 1.5f;
    public EnemyIdleState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {

        //Debug.Log("Hello From Enemy Idle State");
        StopWalking();
        idleTimer = 0f;


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
        //enemy.CheckMovementDirection();
        // Kiểm tra nếu không có player trong tầm nhìn
        if (!enemy.isPlayerInAttackRange)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer > idleDuration) {
                FlipEnemy();
                enemy.enemyStateMachine.ChangeState(enemy.patrolState);
                idleTimer = 0f;
                //Debug.Log("1s passed");
            }
        }
        else
        {
            idleTimer = 0f;
        }
        if (CheckIfCanChase()) enemy.enemyStateMachine.ChangeState(enemy.chaseState);


    }

    public void StopWalking()
    {
        enemy.moveDirection.x = 0f;
        enemy.animator.SetFloat("speed", Mathf.Abs(enemy.moveDirection.x));
    }
    private bool CheckIfCanChase()
    {
        if (enemy.isPlayerInDetectionRange)
        {
            return true;
        }
        return false;
    }
    private void FlipEnemy()
    {
        enemy.transform.localScale = new Vector3(-enemy.transform.localScale.x, 1, 1);
        enemy.isFacingRight = !enemy.isFacingRight;
        enemy.moveDirection.x = enemy.isFacingRight ? 1f : -1f;
    }
}
