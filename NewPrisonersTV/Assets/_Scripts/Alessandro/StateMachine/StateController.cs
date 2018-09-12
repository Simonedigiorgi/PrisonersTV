using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace StateMachine
{
    public class StateController : MonoBehaviour
    {

        public State currentState;
        public Decision checkIfGameActive;

        [HideInInspector] public State inactiveState; 
        [HideInInspector] public State remainState;

        [HideInInspector] public float stateTimeElapsed;
        [HideInInspector] public State lastActiveState;

        protected bool isActive = true;

        protected virtual void Awake()
        { 
            remainState = (State)Resources.Load("RemainInState");
        }

        // Update is called once per frame
        public virtual void Update()
        {
        }

        public virtual void TransitionToState(State nextState)
        {
        }

        protected void OnExitState()
        {
            stateTimeElapsed = 0;
        }

    }
}
