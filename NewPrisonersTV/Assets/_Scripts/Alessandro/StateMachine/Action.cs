using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    public class _Action : ScriptableObject
    {
        // public abstract void Execute(StateController controller);

        public virtual void Execute(CharacterStateController controller) { }

        //public virtual void Execute(NpcStateController controller) { }

        public virtual void Execute(EnemiesAIStateController controller) { }

        public virtual void Execute(GMStateController controller) { }
    }
}
