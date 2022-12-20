using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private float _EarnMoneyCount;
    [SerializeField] private List<TextMeshProUGUI> _MoneyTexts;
    [Header("Upgrades")]
    [SerializeField] private Button _UpgradeFireRate;
    [SerializeField] private Button _UpgradeBumper;
    [SerializeField] private TextMeshProUGUI _BumperPriceText;
    [SerializeField] private TextMeshProUGUI _FireRatePriceText;
    private const float _MAX_UPGRADE_MONEY = 300;
    private float _FireRatePrice;
    private float _BumperPrice;
    private float _Money;

    private void Awake()
    {
        _Money = PlayerPrefs.GetFloat("Money");
        SetMoneyTexts(_Money);
        _FireRatePrice = 100;
        _BumperPrice = 100;
        _BumperPriceText.text = _BumperPrice.ToString();
        _FireRatePriceText.text = _FireRatePrice.ToString();
    }
    private void OnEnable()
    {
        GameActions.instance._EarnMoney += AddMoney;
        _UpgradeFireRate.onClick.AddListener(UpgradeFireRate);
        _UpgradeBumper.onClick.AddListener(UpgradeBumper);
    }
    private void OnDisable()
    {
        GameActions.instance._EarnMoney -= AddMoney;
        _UpgradeFireRate.onClick.RemoveListener(UpgradeFireRate);
        _UpgradeBumper.onClick.RemoveListener(UpgradeBumper);
    }
    private void Update()
    {
        CheckUpgradePrices();
    }
    private void CheckUpgradePrices()
    {
        CheckBumperPrice();
        CheckFireRatePrice();
    }
    private void CheckFireRatePrice()
    {
        if (_FireRatePrice > _Money || _FireRatePrice > _MAX_UPGRADE_MONEY)
            _UpgradeFireRate.interactable = false;
        else
            _UpgradeFireRate.interactable = true;
    }
    private void CheckBumperPrice()
    {
        if (_BumperPrice > _Money || _BumperPrice > _MAX_UPGRADE_MONEY)
            _UpgradeBumper.interactable = false;
        else
            _UpgradeBumper.interactable = true;
    }
    private void UpgradeFireRate()
    {
        GameActions.instance._UpgradeGun?.Invoke();
        _FireRatePrice += _FireRatePrice;
        _FireRatePriceText.text = _FireRatePrice.ToString();
        if (_FireRatePrice > _MAX_UPGRADE_MONEY)
            _FireRatePriceText.text = "FULL";
    }
    private void UpgradeBumper()
    {
        GameActions.instance._UpgradeBumper?.Invoke();
        _BumperPrice += _BumperPrice;
        _BumperPriceText.text = _BumperPrice.ToString();
        if (_BumperPrice > _MAX_UPGRADE_MONEY)
            _BumperPriceText.text = "FULL";
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
