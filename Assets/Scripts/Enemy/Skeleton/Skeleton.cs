using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    // khởi tạo các State trạng thái của skeleton

    public override void Attack()
    {
        base.Attack();
    }

    public override void Chase()
    {
        base.Chase();

    }

    public override void Move()
    {
        base.Move();

    }

    public override void Patrol()
    {
        base.Patrol();
    }
    public override void FlipDirection()
    {
        base.FlipDirection();

    }


    // Start is called before the first frame update
    public new void Awake()
    {
        base.Awake();
        //enemyStateMachine = new EnemyStateMachine();
        //idleState = new EnemyIdleState(this, enemyStateMachine);
        //patrolState = new EnemyPatrolState(this, enemyStateMachine);
        //attackState = new EnemyAttackState(this, enemyStateMachine);
        //deathState = new EnemyDeathState(this, enemyStateMachine);
        //hitState = new EnemyHitState(this, enemyStateMachine);
        //chaseState = new EnemyChaseState(this, enemyStateMachine, playerTransform);
    }
    public new void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    public new void Update()
    {
        base.Update();

    }
    private new void FixedUpdate()
    {
        base.FixedUpdate();

    }
}
