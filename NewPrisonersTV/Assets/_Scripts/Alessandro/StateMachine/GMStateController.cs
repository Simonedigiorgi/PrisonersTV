using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using GM;

namespace StateMachine
{
    public class GMStateController : StateController {

        [HideInInspector] public GMController m_GM;

        protected override void Awake()
        {
            base.Awake();
            m_GM = GetComponent<GMController>();
        }

        public override void TransitionToState(State nextState)
        {
            if (nextState != remainState)
            {
                currentState.OnExitState(this);
                currentState = nextState;
                currentState.OnEnterState(this);
                OnExitState();
            }
        }

        public override void Update()
        {
            base.Update();
            currentState.UpdateState(this);
            //Debug.Log(m_GM.isGameActive);
        }

    }
}
