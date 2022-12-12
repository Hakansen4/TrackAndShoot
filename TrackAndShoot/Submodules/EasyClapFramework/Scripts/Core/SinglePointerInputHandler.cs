using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EasyClap.Common.Input
{
    public abstract class SinglePointerInputHandler : MonoBehaviour, IPointerDownHandler, IDragHandler,
        IPointerUpHandler
    {
        private const int NullPointerID = Int32.MinValue;
        private int _pointerId = NullPointerID;

        public bool Input => _pointerId != NullPointerID;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_pointerId != NullPointerID)
            {
                return;
            }

            _pointerId = eventData.pointerId;
            OnSinglePointerDown(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_pointerId != eventData.pointerId)
            {
                return;
            }

            OnSinglePointerDrag(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_pointerId != eventData.pointerId)
            {
                return;
            }

            _pointerId = NullPointerID;
            OnSinglePointerUp(eventData);
        }

        public abstract void OnSinglePointerDown(PointerEventData eventData);

        public abstract void OnSinglePointerDrag(PointerEventData eventData);

        public abstract void OnSinglePointerUp(PointerEventData eventData);

        private void OnDisable()
        {
            if (Input)
            {
                var eventData = new PointerEventData(EventSystem.current);
                eventData.pointerId = _pointerId;
                OnPointerUp(eventData);
            }
        }
    }
}
