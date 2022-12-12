using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace EasyClap.Seneca.Common.Utility
{
    [Serializable]
    public class BaseTweenSettings
    {
        [CustomValueDrawer("DrawDuration")] public float Duration;
        [SerializeField] private float delay;
        [SerializeField] private bool speedBased;
        [SerializeField] private bool customCurve;
        [SerializeField, HideIf(nameof(customCurve))] private Ease ease;
        [SerializeField, ShowIf(nameof(customCurve))] private AnimationCurve curve;

        public float Speed
        {
            get
            {
                Assert.IsTrue(speedBased);
                return Duration;
            }
        }
        
        public Tween ApplySettings(Tween tween)
        {
            tween.SetDelay(delay);
            if (customCurve)
            {
                tween.SetEase(curve);
            }
            else
            {
                tween.SetEase(ease);
            }

            if (speedBased)
            {
                tween.SetSpeedBased(true);
            }

            return tween;
        }

#if UNITY_EDITOR
        private float DrawDuration(float value, GUIContent label)
        {
            var name = speedBased ? "Speed" : "Duration";
            return EditorGUILayout.FloatField(name, value);
        }
#endif
    }
}