using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EasyClap.Common.Input
{
    public class SinglePointerRunnerInputHandler : SinglePointerInputHandler
    {
        private List<ISinglePointerRunnerInputListener> _listeners = new List<ISinglePointerRunnerInputListener>();

        public override void OnSinglePointerDown(PointerEventData eventData)
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i].OnInputStart();
            }
        }

        public override void OnSinglePointerDrag(PointerEventData eventData)
        {
            var cumulativeDrag = CalculateCumulativeDrag(eventData);
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i].OnDrag(cumulativeDrag);
            }
        }

        public override void OnSinglePointerUp(PointerEventData eventData)
        {
            var cumulativeDrag = CalculateCumulativeDrag(eventData);
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                var listener = _listeners[i];
                listener?.OnDrag(cumulativeDrag);
                listener?.OnInputEnd();
            }
        }

        private Vector2 CalculateCumulativeDrag(PointerEventData eventData)
        {
            return (eventData.position - eventData.pressPosition) / Screen.dpi;
        }

        public void AddListener(ISinglePointerRunnerInputListener listener)
        {
            if (Input)
            {
                listener.OnInputStart();
            }
            
            _listeners.Add(listener);
        }

        public void RemoveListener(ISinglePointerRunnerInputListener listener)
        {
            if (Input)
            {
                listener.OnInputEnd();
            }
            
            _listeners.Remove(listener);
        }
    }
}