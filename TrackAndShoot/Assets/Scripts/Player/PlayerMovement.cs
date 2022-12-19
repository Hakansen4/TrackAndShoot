using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _Speed;
    [Header("Wheels")]
    [SerializeField] private Transform _LeftWheel;
    [SerializeField] private Transform _RightWheel;
    [Header("Boundaries")]
    [SerializeField] private float _LeftBoundarie;
    [SerializeField] private float _RightBoundarie;
    private const float _ROTATE_VALUE = 20.0f;
    private bool _IsShootingStarted = false;
    private bool _IsGameStarted;
    private void OnEnable()
    {
        GameActions.instance._StartDriveAction += StartMovement;
        GameActions.instance._StartShootAction += StartShooting;
        GameActions.instance._LevelCompleted += LevelFinished;
        GameActions.instance._LevelFailed += LevelFinished;
    }
    private void OnDisable()
    {
        GameActions.instance._StartDriveAction -= StartMovement;
        GameActions.instance._StartShootAction -= StartShooting;
        GameActions.instance._LevelCompleted -= LevelFinished;
        GameActions.instance._LevelFailed -= LevelFinished;
    }
    private void LevelFinished()
    {
        _IsGameStarted = false;
    }
    private void StartMovement()
    {
        _IsGameStarted = true;
    }
    private void StartShooting()
    {
        _IsShootingStarted = true;
        CheckRotate(0);
        MoveToCenter();
    }
    private void Update()
    {
        if (_IsGameStarted)
            Movement();
    }
    private void Movement()
    {
        MoveForward();
        MoveSides();
    }
    private void MoveForward()
    {
        transform.position += Vector3.forward * _Speed * Time.deltaTime;
    }
    private void MoveSides()
    {
        if (_IsShootingStarted)
            return;

        float _Horizontal = Input.GetAxis("Horizontal");
        CheckRotate(_Horizontal);
        float _Xvalue = (_Horizontal * _Speed * Time.deltaTime) + transform.position.x;
        _Xvalue = Mathf.Clamp(_Xvalue, _LeftBoundarie, _RightBoundarie);
        transform.position = new Vector3(_Xvalue, transform.position.y, transform.position.z);
    }
    private void MoveToCenter()
    {
        transform.DOMoveX(0, 1.5f);
    }
    private void CheckRotate(float HorizontalValue)
    {
        transform.rotation = Quaternion.Euler(Vector3.up * HorizontalValue * _ROTATE_VALUE);
        _RightWheel.rotation = Quaternion.Euler(Vector3.up * HorizontalValue * _ROTATE_VALUE);
        _RightWheel.Rotate(Vector3.up * HorizontalValue * _ROTATE_VALUE);
        _LeftWheel.rotation = Quaternion.Euler(Vector3.up * HorizontalValue * _ROTATE_VALUE);
        _LeftWheel.Rotate(Vector3.up * HorizontalValue * _ROTATE_VALUE);
    }
}
