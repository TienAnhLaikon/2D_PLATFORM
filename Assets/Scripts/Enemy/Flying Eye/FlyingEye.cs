using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : FlyingEnemy
{
    // các state độc quyên của FLyingEye
    public override void Attack()
    {
        base.Attack();
    }

    public override void Chase()
    {
        base.Chase();
    }

    public override void FlipDirection()
    {
        base.FlipDirection();
        moveDirection.x = -moveDirection.x;
    }

    public override void Flight()
    {
        base.Flight();

    }

    public override void Patrol()
    {
        base.Patrol();
    }
    public new void Awake()
    {
        base.Awake();
        enemyStateMachine = new EnemyStateMachine();
        flyingEyeFlightState = new FlyingEyeFlightState(this, enemyStateMachine);
        flyingEyePatrolState = new FlyingEyePatrolState(this, enemyStateMachine);
        flyingEyeAttackState = new FlyingEyeAttackState(this, enemyStateMachine);
        flyingEyeChaseState = new FlyingEyeChaseState(this, enemyStateMachine,playerTransform);
        flyingEyeHitState = new FlyingEyeHitState(this, enemyStateMachine);
        flyingEyeDeathState = new FlyingEyeDeathState(this, enemyStateMachine);

    }
    // Start is called before the first frame update
    public new void Start()
    {
        base.Start();
        enemyStateMachine.Initialize(flyingEyePatrolState);
        SetRandomMoveDirectionLeftOrRight();
        if (moveDirection.x > 0 && !isFacingRight)
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            isFacingRight = !isFacingRight;
        }
        else if (moveDirection.x < 0 && isFacingRight)
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            isFacingRight = !isFacingRight;
        }
    }

    // Update is called once per frame
    public new void Update()
    {
        base.Update();
        enemyStateMachine.currentState.Update();
    }
    private void FixedUpdate()
    {
        enemyStateMachine.currentState.FixedUpdate();

    }
}
