using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using Character;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/KamikazeDeath")]
    public class En_KamikazeDeath : _Action
    {
        public override void Execute(EnemiesAIStateController controller)
        {
            Life(controller);
        }

        public void Life(EnemiesAIStateController controller)
        {
            controller.m_EnemyController.agent.isStopped = true;
            //enemy die
            if (controller.m_EnemyController.canExplode || controller.enemyStats.enemyLevel >= 2)
                controller.m_EnemyController.enemyAnim.SetTrigger("Explosion");
            else
                controller.m_EnemyController.startDieCoroutine = true;
        }
    }
}
