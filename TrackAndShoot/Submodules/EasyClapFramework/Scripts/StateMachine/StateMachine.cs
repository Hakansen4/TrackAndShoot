using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;

namespace EasyClap.Seneca.StateMachine
{
    public class StateMachine : MonoBehaviour
    {
        [InfoBox("States in the array are assumed to be in order.")]
        [SerializeField] private State[] states;

        private int _currentStateIndex;
        private bool _started;

        [ShowInInspector] 
        public State CurrentState
        {
            get
            {
                if (_currentStateIndex < 0 || _currentStateIndex >= states.Length)
                {
                    return null;
                }
                return states[_currentStateIndex];
            }
        }

        private void Awake()
        {
            foreach (var state in states)
            {
                state.Setup(this);
            }
        }

        private void OnEnable()
        {
            if (_started)
            {
                states[_currentStateIndex].Enter();
            }
        }

        private void Start()
        {
            _started = true;
            states[_currentStateIndex].Enter();
        }
        
        private void OnDisable()
        {
            states[_currentStateIndex].Exit();
        }
        
        [Button]
        public void TransitionToNextState()
        {
            TransitionAt((_currentStateIndex + 1) % states.Length);
        }
        
        [Button]
        public void TransitionTo(State state)
        {
            for (int i = 0; i < states.Length; i++)
            {
                if (state == states[i])
                {
                    TransitionAt(i);
                    return;
                }
            }

            throw new Exception($"Given {nameof(State)} does not exist in {nameof(StateMachine)}!");
        }

        [Button]
        public void TransitionAt(int index)
        {
            Assert.IsTrue(index >= 0);
            Assert.IsTrue(index < states.Length);
            Assert.IsTrue(index != _currentStateIndex);

            var oldIndex = _currentStateIndex;
            _currentStateIndex = index;
            states[oldIndex].Exit(); 
            states[_currentStateIndex].Enter();
        }
    }
}