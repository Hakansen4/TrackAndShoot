using JetBrains.Annotations;
using UnityEngine;

namespace EasyClap.Seneca.Common.Utility
{
    /// <summary>
    /// Base class for a MonoBehaviour that runs an action at the beginning of the component lifecycle.
    /// </summary>
    /// <remarks>
    /// Exact timing is determined in the inspector, by a serialized field.
    /// </remarks>
    [PublicAPI]
    public abstract class InitActionMonoBehaviour : MonoBehaviour
    {
        [SerializeField]
        private ComponentInitEventType actionTime = ComponentInitEventType.Start;

        void Awake()
        {
            if (actionTime == ComponentInitEventType.Awake)
            {
                PerformAction();
            }
        }

        void OnEnable()
        {
            if(actionTime == ComponentInitEventType.OnEnable)
            {
                PerformAction();
            }
        }

        void Start()
        {
            if(actionTime == ComponentInitEventType.Start)
            {
                PerformAction();
            }
        }

        protected abstract void PerformAction();
    }
}
