using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeathState : BossStateBase
{
    public BossDeathState(Boss boss, BossStateMachine bossStateMachine) : base(boss, bossStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Hello from enemy death state");
        boss.animator.SetBool("IsAlive", false);
        UIManager.Instance.bossHealth_Bar.Hide();

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
