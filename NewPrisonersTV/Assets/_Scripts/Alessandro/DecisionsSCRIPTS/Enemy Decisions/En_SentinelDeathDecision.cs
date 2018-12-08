using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace Character.Decisions
{
    [CreateAssetMenu(menuName = "StateMachine/Decisions/Enemy/En_SentinelDeathDecision")]
    public class En_SentinelDeathDecision : Decision
    {
        public override bool Decide(EnemiesAIStateController controller)
        {
            if (controller.m_EnemyController.currentLife <= 0)
                return true;
            else
                return false;
        }
    }
}