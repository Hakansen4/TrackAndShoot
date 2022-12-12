using System;
using UnityEngine;

namespace EasyClap.Seneca.StateMachine
{
    public abstract class State : MonoBehaviour
    {
        protected StateMachine StateMachine;
        
        public void Setup(StateMachine stateMachine)
        {
            StateMachine = stateMachine;
            enabled = false;
        }
        
        public void Enter()
        {
            enabled = true;
            OnEnter();
        }

        public void Exit()
        {
            enabled = false;
            OnExit();
        }

        protected abstract void OnEnter();
        
        protected abstract void OnExit();
    }
}