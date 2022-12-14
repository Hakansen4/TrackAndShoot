using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _Speed;
    [Header("Boundaries")]
    [SerializeField] private float _LeftBoundarie;
    [SerializeField] private float _RightBoundarie;
    private bool _IsGameStarted;
    private void OnEnable()
    {
        GameActions.instance._StartDriveAction += StartMovement;
    }
    private void OnDisable()
    {
        GameActions.instance._StartDriveAction -= StartMovement;
    }
    private void StartMovement()
    {
        _IsGameStarted = true;
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
        float _Horizontal = Input.GetAxis("Horizontal");
        float _Xvalue = (_Horizontal * _Speed * Time.deltaTime) + transform.position.x;
        _Xvalue = Mathf.Clamp(_Xvalue, _LeftBoundarie, _RightBoundarie);
        transform.position = new Vector3(_Xvalue, transform.position.y, transform.position.z);
    }
}
