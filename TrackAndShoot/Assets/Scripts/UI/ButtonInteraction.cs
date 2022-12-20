using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonInteraction : MonoBehaviour
{
    [SerializeField] private Button _StartGameButton;
    
    [SerializeField] private Button _RetryButton;
    [SerializeField] private Button _PlayAgain;
    private void OnEnable()
    {
        _StartGameButton.onClick.AddListener(StartTheGame);
        _RetryButton.onClick.AddListener(ReloadLevel);
        _PlayAgain.onClick.AddListener(ReloadLevel);
    }
    private void OnDisable()
    {
        _StartGameButton.onClick.RemoveListener(StartTheGame);
        _PlayAgain.onClick.RemoveListener(ReloadLevel);
        _RetryButton.onClick.RemoveListener(ReloadLevel);
    }
    private void StartTheGame()
    {
        StateManager.instance.SwitchState(StateManager.instance._DriveGameState);
    }
    
    private void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
