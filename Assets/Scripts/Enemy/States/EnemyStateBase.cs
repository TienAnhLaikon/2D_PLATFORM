using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyStateBase
{
    protected Enemy enemy;
    protected FlyingEnemy flyingEnemy;
    protected EnemyStateMachine enemyStateMachine;

    public EnemyStateBase (Enemy enemy, EnemyStateMachine enemyStateMachine)
    {
        this.enemy = enemy;
        this.enemyStateMachine = enemyStateMachine;
    }
    public EnemyStateBase(FlyingEnemy flyingEnemy, EnemyStateMachine enemyStateMachine)
    {
        this.flyingEnemy = flyingEnemy;
        this.enemyStateMachine = enemyStateMachine;
    }
    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
}
