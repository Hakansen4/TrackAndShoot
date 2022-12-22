using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance { get; private set; }
    private const float _RESET_SPEED = 20;
    private bool _ResetDragValue;
    private Vector3 _DragLastPosition;
    private float _DragValue;
    private float _DragGunValue;
    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }
    public float GetDragValue()
    {
        return _DragValue;
    }
    public float GetDragGunValue()
    {
        return _DragGunValue;
    }
    private void Update()
    {
        CalculateDragValue();
    }
    private void FixedUpdate()
    {
        if (_ResetDragValue)
            ResetDragValue();
    }
    private void CalculateDragValue()
    {
        #if UNITY_EDITOR
        if(Input.GetMouseButtonDown(0))
        {
            _ResetDragValue = false;
            _DragLastPosition = Input.mousePosition;
        }
        else if(Input.GetMouseButton(0))
        {
            _DragValue = Input.mousePosition.x - _DragLastPosition.x;
            _DragGunValue = Input.mousePosition.x - _DragLastPosition.x;
        }
        if(Input.GetMouseButtonUp(0))
        {
            _DragLastPosition = Vector3.zero;
            _DragGunValue = 0;
            _ResetDragValue = true;
        }
        #endif
        if (Input.touchCount > 0    &&  Input.touches[0].phase == TouchPhase.Began)
        {
            _ResetDragValue = false;
            _DragLastPosition = Input.GetTouch(0).position;
        }
        else if (Input.touchCount > 0)
        {
            _DragValue = Input.touches[0].position.x - _DragLastPosition.x;
            _DragGunValue = Input.touches[0].position.x - _DragLastPosition.x;
        }
        if (Input.touchCount > 0   &&  Input.touches[0].phase == TouchPhase.Ended)
        {
            _DragLastPosition = Vector3.zero;
            _DragGunValue = 0;
            _ResetDragValue = true;
        }
    }
    private void ResetDragValue()
    {
        if (_DragValue > 0.1f)
            _DragValue -= _RESET_SPEED;
        else if (_DragValue < -0.1f)
            _DragValue += _RESET_SPEED;
        else
            _DragValue = 0;
    }
}