using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : BossStateBase
{
    public BossIdleState(Boss boss, BossStateMachine bossStateMachine) : base(boss, bossStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        // Debug.Log("Hello from Boss Idle State");
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
        if (CheckIfCanChase()) boss.bossStateMachine.ChangeState(boss.chaseState);

    }
    private bool CheckIfCanChase() {
        if (boss.isPlayerInDetectionRange)
        {
            return true;
        }
        return false;
    }
}
