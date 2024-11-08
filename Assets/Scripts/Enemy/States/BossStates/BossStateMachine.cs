using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateMachine
{
    public BossStateBase currentState { get; set; }

    public void Initialize(BossStateBase startingState)
    {
        if (startingState != null)
        {
            currentState = startingState;
            currentState.EnterState();
        }
        else
        {
            Debug.LogError("Starting state is null.");
        }
    }
    public void ChangeState(BossStateBase newState)
    {
        // kiểm tra nếu trạng thái mới khác trạng thái hiện tại
        if (newState != null && newState != currentState)
        {
            currentState.ExitState();
            currentState = newState;
            currentState.EnterState();
        }
        else
        {
            Debug.LogError("New state is null.");
        }
    }
}
