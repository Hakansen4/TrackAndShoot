using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockCamera : MonoBehaviour
{
    [SerializeField] private float _XValue;
    private void Update()
    {
        LockPositionX();
    }
    private void LockPositionX()
    {
        transform.position = new Vector3(_XValue, transform.position.y, transform.position.z);
    }
}
