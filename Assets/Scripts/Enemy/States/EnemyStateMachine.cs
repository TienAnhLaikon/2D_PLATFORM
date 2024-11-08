using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public EnemyStateBase currentState { get; set; }

    public void Initialize(EnemyStateBase startingState)
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
    public void ChangeState(EnemyStateBase newState)
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
