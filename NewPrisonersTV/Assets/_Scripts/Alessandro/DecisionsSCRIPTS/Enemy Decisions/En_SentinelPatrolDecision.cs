using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace Character.Decisions
{
    [CreateAssetMenu(menuName = "StateMachine/Decisions/Enemy/En_SentinelPatrolDecision")]
    public class En_SentinelPatrolDecision : Decision
    {
        public override bool Decide(EnemiesAIStateController controller)
        {
            if (!controller.m_EnemyController.playerSeen) 
                return true;
            else
                return false;
        }
    }
}