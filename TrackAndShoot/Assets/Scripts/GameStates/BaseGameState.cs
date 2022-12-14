using UnityEngine;

public abstract class BaseGameState
{
    public float speed = 2.0f;
    public abstract void EnterState(StateManager enemy);

    public abstract void UpdateState(StateManager enemy);

}