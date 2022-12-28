using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintController : MonoBehaviour
{
    [SerializeField] private Text _PaintRateText;
    int _PaintedValue;

    private void Update()
    {
        CheckPaintFinished();
    }
    private void CheckPaintFinished()
    {
        _PaintedValue = int.Parse(_PaintRateText.text);
        if (_PaintedValue >= 100)
            StateManager.instance.SwitchState(StateManager.instance._LevelCompletedState);
    }
}
