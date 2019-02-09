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
            // if the timer is up and die from explosion don't give points to players
            if (controller.m_EnemyController.canExplode)
                controller.m_EnemyController.enemyOwnership = -1;
            // if timer is up or is lv 2+ trigger the explosion
            if (controller.m_EnemyController.canExplode || controller.enemyStats.enemyLevel >= 2)           
                controller.m_EnemyController.enemyAnim.SetTrigger("Explosion");
            // if is lv 1 and died from being hit then gives the points to players and die
            else
                controller.m_EnemyController.startDieCoroutine = true;
        }
    }
}
