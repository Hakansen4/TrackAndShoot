using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFailedState : BaseGameState
{
    public override void EnterState(StateManager state)
    {
        state.LevelFinished(true);
        GameActions.instance._LevelFailed?.Invoke();
    }

    public override void UpdateState(StateManager state)
    {
    }
}
