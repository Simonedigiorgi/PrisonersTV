using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/KamikazeAggroExit")]
    public class En_KamikazeAggroExit: _Action
    {
        public override void Execute(EnemiesAIStateController controller)
        {
            Attack(controller);
        }

        public void Attack(EnemiesAIStateController controller)
        {
            controller.m_EnemyController.agent.enabled = true;
            //controller.m_EnemyController.agent.isStopped = false;
        }
    }
}
