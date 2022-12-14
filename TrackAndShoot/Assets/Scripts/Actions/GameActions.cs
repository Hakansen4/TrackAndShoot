using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameActions : MonoBehaviour
{
    public static GameActions instance { get; private set; }

    //public UnityAction<int, int> _PressedAction;
    public UnityAction _StartDriveAction;
    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }
}
