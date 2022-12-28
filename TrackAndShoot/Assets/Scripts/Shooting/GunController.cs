using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour,IUpgradable
{
    [SerializeField] private float _RotationValue;
    [SerializeField] private float _Range;
    [SerializeField] private float _RayThickness;
    [SerializeField] private float _FireRate;
    [SerializeField] private GameObject[] _Guns;
    [SerializeField] private ParticleSystem _FireEffect;
    [SerializeField] private AudioSource _FireSound;
    private const float _MAX_ROTATE_VALUE = 0.6f;
    private float _FireTimer;
    private bool _IsShootingStarted;
    private RaycastHit _Hit;
    private void OnEnable()
    {
        GameActions.instance._StartShootAction += StartShooting;
        GameActions.instance._UpgradeGun += Upgrade;
        GameActions.instance._LevelCompleted += StopShooting;
        GameActions.instance._LevelFailed += StopShooting;
        GameActions.instance._StartPainting += StopShooting;
        _FireEffect.Stop();
    }
    private void OnDisable()
    {
        GameActions.instance._StartShootAction -= StartShooting;
        GameActions.instance._UpgradeGun -= Upgrade;
        GameActions.instance._LevelCompleted -= StopShooting;
        GameActions.instance._LevelFailed -= StopShooting;
        GameActions.instance._StartPainting -= StopShooting;
    }
    private void StartShooting()
    {
        _IsShootingStarted = true;
    }
    private void StopShooting()
    {
        _IsShootingStarted = false;
    }
    private void Update()
    {
        if (!_IsShootingStarted)
            return;
        RotateGun();
        ShootTheGun();
    }
    private void RotateGun()
    {
        float _Horizontal = InputManager.instance.GetDragGunValue();
        _Horizontal = _Horizontal * _RotationValue;
        _Horizontal = Mathf.Clamp(_Horizontal, -_MAX_ROTATE_VALUE - transform.rotation.y, _MAX_ROTATE_VALUE - transform.rotation.y);
        transform.Rotate(Vector3.up * _Horizontal);
    }
    private void ShootTheGun()
    {
        if (Time.time - _FireTimer < _FireRate)
            return;

        Ray _Ray = new Ray();
        _Ray.origin = transform.position;
        _Ray.direction = transform.TransformDirection(Vector3.back);
        _FireEffect.Play();
        _FireSound.Play();
        if(Physics.SphereCast(_Ray,_RayThickness,out _Hit,10))
        {
            if (_Hit.collider.CompareTag("Enemy"))
            {
                _Hit.collider.GetComponent<HealthBarEnemy>().TakeBulletDamage();
            }
        }
        _FireTimer = Time.time;
    }

    public void Upgrade()
    {
        if (_FireRate <= 0.05f)
            return;
        _FireRate -= 0.15f;
        for (int i = 0; i < _Guns.Length; i++)
        {
            if(_Guns[i].active)
            {
                _Guns[i].SetActive(false);
                _Guns[i + 1].SetActive(true);
                return;
            }
        }
    }
}
