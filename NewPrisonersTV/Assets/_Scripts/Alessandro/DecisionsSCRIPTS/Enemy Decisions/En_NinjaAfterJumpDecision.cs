using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace Character.Decisions
{
    [CreateAssetMenu(menuName = "StateMachine/Decisions/Enemy/NinjaAfterJumpDecision")]
    public class En_NinjaAfterJumpDecision : Decision
    {
        public override bool Decide(EnemiesAIStateController controller)
        {
            if (!controller.m_EnemyController.onGround)
                return true; 
            else
                return false;
        }
    }
}