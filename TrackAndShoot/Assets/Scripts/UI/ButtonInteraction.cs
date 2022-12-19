using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonInteraction : MonoBehaviour
{
    [SerializeField] private Button _StartGameButton;
    [SerializeField] private Button _UpgradeFireRate;
    [SerializeField] private Button _UpgradeBumper;
    [SerializeField] private Button _RetryButton;
    [SerializeField] private Button _PlayAgain;
    private void OnEnable()
    {
        _StartGameButton.onClick.AddListener(StartTheGame);
        _UpgradeFireRate.onClick.AddListener(UpgradeFireRate);
        _UpgradeBumper.onClick.AddListener(UpgradeBumper);
        _RetryButton.onClick.AddListener(ReloadLevel);
        _PlayAgain.onClick.AddListener(ReloadLevel);
    }
    private void OnDisable()
    {
        _StartGameButton.onClick.RemoveListener(StartTheGame);
        _UpgradeFireRate.onClick.RemoveListener(UpgradeFireRate);
        _UpgradeBumper.onClick.RemoveListener(UpgradeBumper);
        _RetryButton.onClick.RemoveListener(ReloadLevel);
        _PlayAgain.onClick.RemoveListener(ReloadLevel);
    }
    private void StartTheGame()
    {
        StateManager.instance.SwitchState(StateManager.instance._DriveGameState);
    }
    private void UpgradeFireRate()
    {
        GameActions.instance._UpgradeGun?.Invoke();
    }
    private void UpgradeBumper()
    {
        GameActions.instance._UpgradeBumper?.Invoke();
    }
    private void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
