using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace Character.Decisions
{
    [CreateAssetMenu(menuName = "StateMachine/Decisions/Enemy/En_DogPatrolDecision")]
    public class En_DogPatrolDecision : Decision
    {
        public override bool Decide(EnemiesAIStateController controller)
        {       // if the target is not in sight or is not alive 
            if (!controller.m_EnemyController.playerSeen || !GMController.instance.playerInfo[controller.m_EnemyController.playerSeenIndex].playerController.isAlive)
                return true;
            else
                return false;
        }
    }
}