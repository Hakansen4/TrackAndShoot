using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompletedState : BaseGameState
{
    public override void EnterState(StateManager state)
    {
        state.LevelFinished(false);
        GameActions.instance._LevelCompleted?.Invoke();
    }

    public override void UpdateState(StateManager state)
    {
    }
}
