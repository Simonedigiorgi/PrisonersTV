using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/SpiderAttack")]
    public class En_SpiderAttack : _Action
    {
        public override void Execute(EnemiesAIStateController controller)
        {
            Attack(controller);
        }

        public void Attack(EnemiesAIStateController controller)
        {
            if (controller.m_EnemyController.currentPathTimer <= 0)
            {
                controller.m_EnemyController.agent.destination = GMController.instance.playerInfo[controller.m_EnemyController.playerSeenIndex].player.transform.position;
                controller.m_EnemyController.currentPathTimer = controller.enemyStats.pathCheckFrequenzy;
            }             

            if (controller.m_EnemyController.agent.isOnOffMeshLink)
               controller.m_EnemyController.agent.speed = controller.enemyStats.jumpSpeed;
            else
                controller.m_EnemyController.agent.speed = controller.enemyStats.runSpeed; 
            //---------------------------------------------------------------------------------------

            if (!controller.m_EnemyController.mine.isActive)
            {
                //drop mines
                controller.m_EnemyController.mine.isActive = true;
            }           
        }

    }
}
