using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace Character.Decisions
{
    [CreateAssetMenu(menuName = "StateMachine/Decisions/Characters/ExitDashDecision")]
    public class Ch_ExitDashDecision: Decision
    {
        public override bool Decide(CharacterStateController controller)
        {
            if (!controller.m_CharacterController.isInDash)
                return true;
            else
                return false;
        }
    }
}