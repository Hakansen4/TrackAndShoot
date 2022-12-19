using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarEnemy : MonoBehaviour,IDie
{
    [SerializeField] private Slider _HealthBar;
    [SerializeField] private float _MaxHealth;
    private float _Health;
    private float _OldHealth;
    private float _Timer;
    private void OnEnable()
    {
        GameActions.instance._StartShootAction += ActivateHealthBar;
    }
    private void OnDisable()
    {
        GameActions.instance._StartShootAction -= ActivateHealthBar;
    }
    private void ActivateHealthBar()
    {
        _HealthBar.gameObject.SetActive(true);
    }
    private void Awake()
    {
        _Health = _MaxHealth;
        _HealthBar.maxValue = _MaxHealth;
        _HealthBar.value = _MaxHealth;
        _OldHealth = _MaxHealth;
    }
    private void Update()
    {
        UpdateHealthBar();
    }
    private void UpdateHealthBar()
    {
        if(_OldHealth != _Health    &&  Time.time - _Timer > 0.0025f)
        {
            _Timer = Time.time;
            _OldHealth--;
            _HealthBar.value = _OldHealth;
        }
    }
    public void TakeBulletDamage()
    {
        _Health -= 25;
        if (_Health <= 0)
            Die();
    }

    public void Die()
    {
        GameActions.instance._EarnMoney?.Invoke();
        gameObject.SetActive(false);
    }
}
