using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInteraction : MonoBehaviour
{
    [SerializeField] private Button _StartGameButton;
    private void OnEnable()
    {
        _StartGameButton.onClick.AddListener(StartTheGame);
    }
    private void OnDisable()
    {
        _StartGameButton.onClick.RemoveListener(StartTheGame);
    }
    private void StartTheGame()
    {
        StateManager.instance.SwitchState(StateManager.instance._DriveGameState);
    }
}
