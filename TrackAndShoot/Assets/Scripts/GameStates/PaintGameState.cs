using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintGameState : BaseGameState
{
    public override void EnterState(StateManager state)
    {
        GameActions.instance._StartPainting?.Invoke();
        state.OpenNextCanvas();
    }

    public override void UpdateState(StateManager state)
    {
    }
}
