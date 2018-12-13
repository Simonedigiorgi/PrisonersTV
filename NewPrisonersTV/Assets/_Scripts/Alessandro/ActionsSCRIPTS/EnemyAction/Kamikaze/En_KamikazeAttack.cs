using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/KamikazeAttack")]
    public class En_KamikazeAttack : _Action
    {
        public override void Execute(EnemiesAIStateController controller)
        {
            Attack(controller);
        }

        public void Attack(EnemiesAIStateController controller)
        {
            if (controller.m_EnemyController.currentViewTimer <= 0)
            {
                controller.m_EnemyController.agent.destination = GMController.instance.playerInfo[controller.m_EnemyController.playerSeenIndex].player.transform.position;
                controller.m_EnemyController.currentViewTimer = controller.enemyStats.viewCheckFrequenzy;
            }

            if (controller.m_EnemyController.agent.isOnOffMeshLink)
               controller.m_EnemyController.agent.speed = controller.enemyStats.jumpSpeed;
            else
                controller.m_EnemyController.agent.speed = controller.enemyStats.runSpeed;
            //---------------------------------------------------------------------------------------

            if (controller.enemyStats.enemyLevel == 3 && !controller.m_EnemyController.mine.isActive)
            {
                //drop bombs
                controller.m_EnemyController.mine.isActive = true;
            }
            // explosions timer
            controller.m_EnemyController.currentExplosionTimer -= Time.deltaTime;
           
            if (controller.m_EnemyController.currentExplosionTimer <= 0 )
            {
                controller.m_EnemyController.canExplode = true;
            }
        }

    }
}
