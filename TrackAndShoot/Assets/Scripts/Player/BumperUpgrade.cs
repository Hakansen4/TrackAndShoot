using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperUpgrade : MonoBehaviour, IUpgradable
{
    [SerializeField] private GameObject _Bumper;
    private void OnEnable()
    {
        GameActions.instance._UpgradeBumper += Upgrade;
    }
    private void OnDisable()
    {
        GameActions.instance._UpgradeBumper -= Upgrade;
    }
    public void Upgrade()
    {
        Vector3 _scale = _Bumper.transform.localScale;
        _scale = new Vector3(_scale.x * 1.2f, _scale.y, _scale.z);
        _Bumper.transform.localScale = _scale;
    }
}
