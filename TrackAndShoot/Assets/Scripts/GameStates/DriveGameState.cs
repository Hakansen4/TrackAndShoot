using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveGameState : BaseGameState
{
    public override void EnterState(StateManager state)
    {
        GameActions.instance._StartDriveAction?.Invoke();
        state.OpenNextCanvas();
    }

    public override void UpdateState(StateManager state)
    {
    }
}
