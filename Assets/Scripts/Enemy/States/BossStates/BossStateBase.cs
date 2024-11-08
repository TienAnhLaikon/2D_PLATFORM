using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossStateBase
{
    protected Boss boss;
    protected BossStateMachine bossStateMachine;

    public BossStateBase(Boss boss, BossStateMachine bossStateMachine)
    {
        this.boss = boss;
        this.bossStateMachine = bossStateMachine;
    }
    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
}
