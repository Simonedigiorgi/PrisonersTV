using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/SightSpider")]
    public class En_SightSpider: _Action
    {
        public override void Execute(EnemiesAIStateController controller) 
        {
            Sight(controller);
        }

        public void Sight(EnemiesAIStateController controller)
        {
            if (controller.enemyStats.enemyLevel >= 2 &&  controller.m_EnemyController.currentViewTimer <= 0)
            {
 
                // check distance between players and enemy
                for (int i = 0; i < GMController.instance.playerInfo.Length; i++)
                    controller.m_EnemyController.playerSeenDistance[i] = new TargetDistance(i, (controller.m_EnemyController.thisTransform.position - GMController.instance.playerInfo[i].playerController.TargetForEnemies.position).sqrMagnitude);

                System.Array.Sort(controller.m_EnemyController.playerSeenDistance);

                // check if the target is in sight based on the distance (check first the closer one)
                for (int i = 0; i < controller.m_EnemyController.playerSeenDistance.Length; i++)
                {   // if the target is in range and is alive 
                    if (controller.m_EnemyController.playerSeenDistance[i].distance <= (controller.enemyStats.attackView * controller.enemyStats.attackView)
                        && GMController.instance.playerInfo[controller.m_EnemyController.playerSeenDistance[i].targetIndex].playerController.isAlive)
                    {
                         Vector2 rayDirection = GMController.instance.playerInfo[controller.m_EnemyController.playerSeenDistance[i].targetIndex].playerController.TargetForEnemies.position - controller.m_EnemyController.thisTransform.position;
                        
                        for (int y = 0; y < controller.m_EnemyController.raycastEyes.Length; y++)
                        {
                            Debug.DrawRay(controller.m_EnemyController.raycastEyes[y].position, rayDirection, Color.red);// use the distance as ray lenght to avoid hitting the floor
                            if (!Physics2D.Raycast(controller.m_EnemyController.raycastEyes[y].position, rayDirection, (controller.m_EnemyController.playerSeenDistance[i].distance/controller.m_EnemyController.playerSeenDistance[i].distance), controller.enemyStats.obstacleMask))                          
                            {
                                Debug.Log("hit"); 
                                controller.m_EnemyController.playerSeenIndex = controller.m_EnemyController.playerSeenDistance[i].targetIndex;
                                controller.m_EnemyController.playerSeen = true;
                            }                               
                        }
                     } 
                }
                controller.m_EnemyController.currentViewTimer = controller.enemyStats.viewCheckFrequenzy;
            }
        }
    }

    
}
