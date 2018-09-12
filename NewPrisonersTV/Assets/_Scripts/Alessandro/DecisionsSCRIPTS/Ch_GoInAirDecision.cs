using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace Character.Decisions
{
    [CreateAssetMenu(menuName = "StateMachine/Decisions/Characters/GoInAir")]
    public class Ch_GoInAirDecision: Decision
    {
        public override bool Decide(CharacterStateController controller)
        {
            if (!controller.m_CharacterController.isGrounded)
                return true;
            else
                return false;
        }
    }
}