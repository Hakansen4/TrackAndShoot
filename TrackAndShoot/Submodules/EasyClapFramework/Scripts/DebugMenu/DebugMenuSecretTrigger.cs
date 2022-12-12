using System;
using System.Collections.Generic;
using EasyClap.Seneca.Common.EventBus;
using UnityEngine;

namespace EasyClap.Seneca.Common
{
    public class DebugMenuSecretTrigger : MonoBehaviour
    {
        [SerializeField] private Pivot pivot;
        [SerializeField] private float radius = 100;
        [SerializeField] private int clickCount;
        [SerializeField] private float duration;

        private Queue<float> _clickTimers;
        
        [RuntimeInitializeOnLoadMethod]
        static void OnRuntimeMethodLoad()
        {
            if (!Application.isEditor && !Debug.isDebugBuild)
            {
                return;
            }
            
            Instantiate(Resources.Load<DebugMenuSecretTrigger>(nameof(DebugMenuSecretTrigger)));
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _clickTimers = new Queue<float>(clickCount);
            EventBus<DebugMenuOpenedEvent>.AddListener(OnDebugMenuOpened);
            EventBus<DebugMenuClosedEvent>.AddListener(OnDebugMenuClosed);
        }

        private void OnDebugMenuOpened(object sender, DebugMenuOpenedEvent e)
        {
            enabled = false;
        }
        
        private void OnDebugMenuClosed(object sender, DebugMenuClosedEvent e)
        {
            enabled = true;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0) && Vector2.Distance(GetPivotPosition(), Input.mousePosition) < radius)
            {
                _clickTimers.Enqueue(Time.unscaledTime);
                if (_clickTimers.Count == clickCount)
                {
                    OpenDebugMenu();
                    _clickTimers.Clear();
                }
            }

            while (_clickTimers.Count > 0 && Time.unscaledTime - _clickTimers.Peek() >= duration )
            {
                _clickTimers.Dequeue();
            }
        }

        private static void OpenDebugMenu()
        {
            var debugMenuPrefab = Resources.Load<DebugMenu>($"{nameof(DebugMenu)}Override");
            if (!debugMenuPrefab)
            {
                debugMenuPrefab = Resources.Load<DebugMenu>(nameof(DebugMenu));
            }
            
            Instantiate(debugMenuPrefab);
        }

        private Vector2 GetPivotPosition()
        {
            switch (pivot)
            {
                case Pivot.TopLeft:
                    return new Vector2(0, Screen.height);
                
                case Pivot.TopRight:
                    return new Vector2(Screen.width, Screen.height);
                
                case Pivot.BottomLeft:
                    return new Vector2(0, 0);
                
                case Pivot.BottomRight:
                    return new Vector2(Screen.width, 0);
                
                default:
                    return new Vector2(0, Screen.height);
            }
        }

        private void OnDestroy()
        {
            EventBus<DebugMenuOpenedEvent>.RemoveListener(OnDebugMenuOpened);
            EventBus<DebugMenuClosedEvent>.RemoveListener(OnDebugMenuClosed);
        }

        [Serializable]
        private enum Pivot
        {
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight
        }
    }
}
