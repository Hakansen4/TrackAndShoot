using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager instance;
    [SerializeField] private List<GameObject> Canvases;
    [SerializeField] private GameObject _LevelFailedCanvas;
    [SerializeField] private GameObject _LevelCompletedCanvas;
    #region States
    private BaseGameState _CurrentState;
    public StartGameState _StartGameState = new StartGameState();
    public DriveGameState _DriveGameState = new DriveGameState();
    public ShootGameState _ShootGameState = new ShootGameState();
    public LevelCompletedState _LevelCompletedState = new LevelCompletedState();
    public LevelFailedState _LevelFailedState = new LevelFailedState();
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
    public void LevelFinished(bool _IsFailed)
    {
        foreach (var canvas in Canvases)
        {
            canvas.SetActive(false);
        }
        if (_IsFailed)
            _LevelFailedCanvas.SetActive(true);
        else
            _LevelCompletedCanvas.SetActive(true);
    }
}