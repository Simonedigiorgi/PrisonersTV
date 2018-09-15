using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace Character.Decisions
{
    [CreateAssetMenu(menuName = "StateMachine/Decisions/Characters/RespawnDecision")]
    public class Ch_RespawnDecision: Decision
    {
        public override bool Decide(CharacterStateController controller)
        {
            if (controller.m_CharacterController.isAlive)
                return true;
            else
                return false;
        }
    }
}