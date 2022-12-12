using UnityEngine;
using UnityEngine.Events;

namespace EasyClap.Seneca.Common.Utility
{
    /// <summary>
    /// Exposes <c>OnTrigger*</c> event functions as <c>onTrigger*</c> UnityEvents.
    /// </summary>
    /// <remarks>
    /// This component has a performance overhead. Try NOT to use it as much as possible.
    /// </remarks>
    public sealed class TriggerEventAdapter : MonoBehaviour
    {
        public UnityEvent<Collider> onTriggerEnter;
        public UnityEvent<Collider> onTriggerStay;
        public UnityEvent<Collider> onTriggerExit;

        void OnTriggerEnter(Collider other)
        {
            onTriggerEnter?.Invoke(other);
        }

        void OnTriggerStay(Collider other)
        {
            onTriggerStay?.Invoke(other);
        }

        void OnTriggerExit(Collider other)
        {
            onTriggerExit?.Invoke(other);
        }
    }
}
