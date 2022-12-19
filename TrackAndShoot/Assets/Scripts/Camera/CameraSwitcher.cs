using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    Animator _Anim;
    private void Awake()
    {
        _Anim = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        GameActions.instance._StartShootAction += SwitchShootCam;
    }
    private void OnDisable()
    {
        GameActions.instance._StartShootAction -= SwitchShootCam;
    }
    private void SwitchShootCam()
    {
        _Anim.Play("ShootCam");
    }
}
