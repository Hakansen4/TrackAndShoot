using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameState : BaseGameState
{
    public override void EnterState(StateManager state)
    {
        state.OpenNextCanvas();
    }

    public override void UpdateState(StateManager state)
    {
    }
}
