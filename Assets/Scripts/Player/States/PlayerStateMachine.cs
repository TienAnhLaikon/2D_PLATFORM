using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerStateBase currentState { get; set; }

    public void Initialize(PlayerStateBase startingState)
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
    public void ChangeState(PlayerStateBase newState)
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
