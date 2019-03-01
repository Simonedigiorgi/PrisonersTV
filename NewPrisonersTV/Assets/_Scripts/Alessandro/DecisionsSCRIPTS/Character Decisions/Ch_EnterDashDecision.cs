using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace Character.Decisions
{
    [CreateAssetMenu(menuName = "StateMachine/Decisions/Characters/EnterDashDecision")]
    public class Ch_EnterDashDecision: Decision
    {
        public override bool Decide(CharacterStateController controller)
        {
            if (Input.GetButtonDown(controller.m_CharacterController.inputMapping.dodgeInput) && !controller.m_CharacterController.isInDash)
                return true;
            else
                return false;
        }
    }
}