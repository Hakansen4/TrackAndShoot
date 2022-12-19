using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootGameState : BaseGameState
{
    public override void EnterState(StateManager state)
    {
        GameActions.instance._StartShootAction?.Invoke();
        state.OpenNextCanvas();
    }

    public override void UpdateState(StateManager state)
    {
    }
}
