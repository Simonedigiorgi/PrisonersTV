using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/SightNinja")]
    public class En_SightNinja: _Action
    {
        public override void Execute(EnemiesAIStateController controller) 
        {
            Sight(controller);
        }

        public void Sight(EnemiesAIStateController controller)
        {
            if (!controller.m_EnemyController.playerSeen && controller.enemyStats.enemyLevel >= 2 && controller.m_EnemyController.currentViewTimer <= 0)
            {
                // check distance between players and enemy
                for (int i = 0; i < GMController.instance.playerInfo.Length; i++)
                    controller.m_EnemyController.playerSeenDistance[i] = new TargetDistance(i, (controller.m_EnemyController.thisTransform.position - GMController.instance.playerInfo[i].playerController.TargetForEnemies.position).sqrMagnitude);

                System.Array.Sort(controller.m_EnemyController.playerSeenDistance);

                controller.m_EnemyController.numRayHitPlayer = 0;
                // check if the target is in sight based on the distance (check first the closer one)
                for (int i = 0; i < controller.m_EnemyController.playerSeenDistance.Length; i++)
                {   // if the target is in range and is alive 
                    if (controller.m_EnemyController.playerSeenDistance[i].distance <= (controller.enemyStats.attackView * controller.enemyStats.attackView)
                        && GMController.instance.playerInfo[controller.m_EnemyController.playerSeenDistance[i].targetIndex].playerController.isAlive)
                    {                        
                        for (int y = 0; y < controller.m_EnemyController.raycastEyes.Length; y++)
                        {                          
                            Debug.DrawLine(controller.m_EnemyController.raycastEyes[y].position, GMController.instance.playerInfo[controller.m_EnemyController.playerSeenDistance[i].targetIndex].playerController.TargetForEnemies.position, Color.red);
                            if (Physics2D.LinecastNonAlloc(controller.m_EnemyController.raycastEyes[y].position, GMController.instance.playerInfo[controller.m_EnemyController.playerSeenDistance[i].targetIndex].playerController.TargetForEnemies.position, controller.m_EnemyController.lineCastHits, controller.enemyStats.obstacleMask) <= 0)
                            {
                                controller.m_EnemyController.playerSeenIndex = controller.m_EnemyController.playerSeenDistance[i].targetIndex;
                                controller.m_EnemyController.numRayHitPlayer++;
                                controller.m_EnemyController.playerSeen = true;
                            }  
                        }
                    } 
                    else if(controller.m_EnemyController.numRayHitPlayer == 0)
                        controller.m_EnemyController.playerSeen = false; 
                }
                controller.m_EnemyController.currentViewTimer = controller.enemyStats.viewCheckFrequenzy;
            }
        }
    }

    
}
