using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private float _EarnMoneyCount;
    [SerializeField] private List<TextMeshProUGUI> _MoneyTexts;
    private float _Money;

    private void Awake()
    {
        _Money = PlayerPrefs.GetFloat("Money");
        SetMoneyTexts(_Money);
    }
    private void OnEnable()
    {
        GameActions.instance._EarnMoney += AddMoney;
    }
    private void OnDisable()
    {
        GameActions.instance._EarnMoney -= AddMoney;
    }
    private void AddMoney()
    {
        _Money += _EarnMoneyCount;
        PlayerPrefs.SetFloat("Money", _Money);
        SetMoneyTexts(_Money);
    }
    private void SetMoneyTexts(float Value)
    {
        foreach (var text in _MoneyTexts)
        {
            text.text = Value.ToString();
        }
    }
}
