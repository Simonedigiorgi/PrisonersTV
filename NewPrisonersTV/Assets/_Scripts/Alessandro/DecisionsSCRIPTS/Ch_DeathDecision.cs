using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace Character.Decisions
{
    [CreateAssetMenu(menuName = "StateMachine/Decisions/Characters/DeathDecision")]
    public class Ch_DeathDecision: Decision
    {
        public override bool Decide(CharacterStateController controller)
        {
            if (!controller.m_CharacterController.isAlive)
                return true;
            else
                return false;
        }
    }
}