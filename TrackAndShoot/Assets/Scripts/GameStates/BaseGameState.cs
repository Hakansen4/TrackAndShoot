using UnityEngine;

public abstract class BaseGameState
{
    public float speed = 2.0f;
    public abstract void EnterState(StateManager state);

    public abstract void UpdateState(StateManager state);

}