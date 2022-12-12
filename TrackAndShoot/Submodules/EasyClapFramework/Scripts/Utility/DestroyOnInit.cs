using JetBrains.Annotations;
using UnityEngine;

namespace EasyClap.Seneca.Common.Utility
{
    [PublicAPI]
    public sealed class DestroyOnInit : InitActionMonoBehaviour
    {
        [SerializeField, Tooltip("If true, uses DestroyImmediate; otherwise uses Destroy.")]
        private bool immediate = false;

        protected override void PerformAction()
        {
            if (immediate)
            {
                Debug.LogWarning($"{nameof(DestroyOnInit)}: calling {nameof(DestroyImmediate)} on {gameObject.name} gameObject.");
                DestroyImmediate(gameObject, false);
            }
            else
            {
                Debug.LogWarning($"{nameof(DestroyOnInit)}: calling {nameof(Destroy)} on {gameObject.name} gameObject.");
                Destroy(gameObject);
            }
        }
    }
}
