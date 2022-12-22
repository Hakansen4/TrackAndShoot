using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCollisionController : MonoBehaviour,IDie
{
    [SerializeField] private int _PlayerMaxHealth;
    [SerializeField] private Slider _PlayerHealthBar;
    [SerializeField] private ParticleSystem _CrushEffect;
    private bool _Shooting;
    private float _OldHealth;
    private float _PlayerHealth;
    private float _Timer;
    private void Awake()
    {
        _PlayerHealthBar.maxValue = _PlayerMaxHealth;
        _PlayerHealthBar.value = _PlayerMaxHealth;
        _OldHealth = _PlayerMaxHealth;
        _PlayerHealth = _PlayerMaxHealth;
        _Timer = 0;
    }
    private void Update()
    {
        UpdateHealthBar();
    }
    public void Die()
    {
        StateManager.instance.SwitchState(StateManager.instance._LevelFailedState);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Obstacle"))
        {
            other.gameObject.SetActive(false);
            _PlayerHealth -= 200;
            _CrushEffect.Play();
            if (_PlayerHealth <= 0)
                Die();
        }
        else if(other.CompareTag("StartShoot"))
        {
            if(!_Shooting)
            {
                _Shooting = true;
                StateManager.instance.SwitchState(StateManager.instance._ShootGameState);
            }
        }
        else if(other.CompareTag("LevelCompleted"))
        {
            StateManager.instance.SwitchState(StateManager.instance._LevelCompletedState);
        }
    }
    private void UpdateHealthBar()
    {
        //DELTA TIME DENE
        if(_OldHealth != _PlayerHealth  &&  Time.time - _Timer > 0.5f)
        {
            _OldHealth--;
            _PlayerHealthBar.value = _OldHealth;
        }
    }
}
