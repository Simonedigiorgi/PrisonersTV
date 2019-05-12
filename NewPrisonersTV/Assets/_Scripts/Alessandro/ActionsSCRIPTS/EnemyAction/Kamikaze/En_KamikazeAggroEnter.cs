using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/KamikazeAggroEnter")]
    public class En_KamikazeAggroEnter: _Action
    {
        public override void Execute(EnemiesAIStateController controller)
        {
            Attack(controller);
        }

        public void Attack(EnemiesAIStateController controller)
        {
            controller.m_EnemyController.isAggroAnim = true;
            controller.m_EnemyController.enemyAnim.SetTrigger("Aggro");
            controller.m_EnemyController.agent.enabled = false;
            //controller.m_EnemyController.agent.isStopped = true;
            controller.m_EnemyController.agent.speed = 0; 
        }
    }
}
