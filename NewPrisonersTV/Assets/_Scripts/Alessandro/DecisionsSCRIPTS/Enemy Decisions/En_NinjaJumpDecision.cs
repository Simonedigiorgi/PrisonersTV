using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace Character.Decisions
{
    [CreateAssetMenu(menuName = "StateMachine/Decisions/Enemy/NinjaJumpDecision")]
    public class En_NinjaJumpDecision : Decision
    {
        public override bool Decide(EnemiesAIStateController controller)
        {
            if (controller.m_EnemyController.currentJumpTimer <= 0 && controller.m_EnemyController.onGround && !controller.m_EnemyController.agent.isOnOffMeshLink)
                return true;
            else
                return false;
        }
    }
}