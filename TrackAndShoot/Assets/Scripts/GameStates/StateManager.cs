using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager instance;
    [SerializeField] private List<GameObject> Canvases;
    #region States
    private BaseGameState _CurrentState;
    public StartGameState _StartGameState = new StartGameState();
    public DriveGameState _DriveGameState = new DriveGameState();
    #endregion
    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }
    private void Start()
    {
        _CurrentState = _StartGameState;
        _CurrentState.EnterState(this);
    }
    private void Update()
    {
        _CurrentState.UpdateState(this);
    }
    public void SwitchState(BaseGameState state)
    {
        _CurrentState = state;
        _CurrentState.EnterState(this);
    }
    public void OpenNextCanvas()
    {
        bool _ThisItem = false;
        GameObject _FirstCanvas = null;
        foreach (var Canvas in Canvases)
        {
            if (_FirstCanvas == null)
                _FirstCanvas = Canvas;
            if(_ThisItem)
            {
                Canvas.SetActive(true);
                return;
            }    
            if(Canvas.active)
            {
                _ThisItem = true;
                Canvas.SetActive(false);
            }
        }
        _FirstCanvas.SetActive(true);
    }
}