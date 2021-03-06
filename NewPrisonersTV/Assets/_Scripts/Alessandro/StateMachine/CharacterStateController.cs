﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Character;

namespace StateMachine
{
    public class CharacterStateController : StateController
    {

        //public CharacterActive thisCharacter;

        [HideInInspector] public CharacterStats characterStats;
        [HideInInspector] public State gameStartState;
        [HideInInspector] public State defeatedState;
        [HideInInspector] public _CharacterController m_CharacterController;
        [HideInInspector] public NavMeshAgent navMeshAgent;

        // Use this for initialization
        protected override void Awake()
        {
            base.Awake();
            inactiveState = (State)Resources.Load("Inactive");
           // navMeshAgent = GetComponent<NavMeshAgent>();

            lastActiveState = currentState;

            m_CharacterController = GetComponent<_CharacterController>();   

            gameStartState = (State)Resources.Load("StartState");           
            defeatedState = (State)Resources.Load("Defeated");

        }

        private void Start()
        {
            characterStats = m_CharacterController.m_CharStats;
        }

        public override void TransitionToState(State nextState)
        {
            if (nextState != remainState)
            {
                currentState.OnExitState(this);
                currentState = nextState;
                currentState.OnEnterState(this);
                if (currentState != inactiveState)
                    lastActiveState = currentState;
                OnExitState();
            }
        }

        public override void Update()
        {
            base.Update();
            // check if the game is inactive (like in a pause state for example) and set the character to and inactive state and then set it back to the previous active state
            if (!checkIfGameActive.Decide(this) && (currentState != inactiveState && currentState != gameStartState && currentState != defeatedState))
            {
                TransitionToState(inactiveState);
            }
            else if (checkIfGameActive.Decide(this) && currentState == inactiveState)
            {
                TransitionToState(lastActiveState);
            }
            //runs the actions of the current state
            currentState.UpdateState(this);

        }

    }
}
