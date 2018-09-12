using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace StateMachine
{
    [System.Serializable]
    public class Transition
    {
        public Decision[] decision;
        public State trueState;
        public State falseState;
    }

}
