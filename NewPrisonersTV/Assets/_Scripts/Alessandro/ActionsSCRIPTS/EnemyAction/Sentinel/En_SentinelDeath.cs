using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using Character;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/SentinelDeath")]
    public class En_SentinelDeath : _Action
    {
        public override void Execute(EnemiesAIStateController controller)
        {
            Life(controller);
        }

        public void Life(EnemiesAIStateController controller)
        {      
            controller.m_EnemyController.startDieCoroutine = true;                               
        }
    }
}
