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
        GameActions.instance._StartPainting += SwitchPaintCam;
    }
    private void OnDisable()
    {
        GameActions.instance._StartShootAction -= SwitchShootCam;
        GameActions.instance._StartPainting -= SwitchPaintCam;
    }
    private void SwitchShootCam()
    {
        _Anim.Play("ShootCam");
    }
    private void SwitchPaintCam()
    {
        _Anim.Play("PaintCam");
    }
}
