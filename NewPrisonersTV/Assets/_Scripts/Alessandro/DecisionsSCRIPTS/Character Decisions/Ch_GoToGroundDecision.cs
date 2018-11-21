using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace Character.Decisions
{
    [CreateAssetMenu(menuName = "StateMachine/Decisions/Characters/GoToGround")]
    public class Ch_GoToGroundDecision: Decision
    {
        public override bool Decide(CharacterStateController controller)
        {
            if (controller.m_CharacterController.isGrounded && controller.m_CharacterController.isAlive)
                return true;
            else
                return false;
        }
    }
}