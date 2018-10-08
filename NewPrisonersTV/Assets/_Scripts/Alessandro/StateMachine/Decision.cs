using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    public class Decision : ScriptableObject {

        public virtual bool Decide(CharacterStateController controller) { return true; }

        //public virtual bool Decide(NpcStateController controller) { return true; }

        public virtual bool Decide(EnemiesAIStateController controller) { return true; }

        public virtual bool Decide(GMStateController controller) { return true; }

    }
}

