using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace Character.Decisions
{
    [CreateAssetMenu(menuName = "StateMachine/Decisions/Enemy/En_SpiderDeathDecision")]
    public class En_SpiderDeathDecision : Decision
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