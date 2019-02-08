using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/BatSight")]
    public class En_Sight: _Action
    {
        public override void Execute(EnemiesAIStateController controller)
        {
            Sight(controller);
        }

        public void Sight(EnemiesAIStateController controller)
        {

            if (controller.enemyStats.enemyLevel == 3 && controller.m_EnemyController.currentViewTimer <= 0)
            {
                if (!controller.m_EnemyController.playerSeen)
                {
                    for (int i = 0; i < GMController.instance.playerInfo.Length; i++)
                    {   // check distance between player and enemy
                        controller.m_EnemyController.playerSeenDistance[i] = new TargetDistance(i,(controller.m_EnemyController.thisTransform.position - GMController.instance.playerInfo[i].playerController.TargetForEnemies.position).sqrMagnitude);
                    }
                    //sort array in crescent order
                    System.Array.Sort(controller.m_EnemyController.playerSeenDistance);
                    // check if the target is in sight based on the distance (check first the closer one)
                    for (int i = 0; i < controller.m_EnemyController.playerSeenDistance.Length; i++)
                    {   // if the target is in range and is alive 
                        if (controller.m_EnemyController.playerSeenDistance[i].distance <= (controller.enemyStats.attackView * controller.enemyStats.attackView)
                            && GMController.instance.playerInfo[controller.m_EnemyController.playerSeenDistance[i].targetIndex].playerController.isAlive)
                        {                           
                            for (int y = 0; y < controller.m_EnemyController.raycastEyes.Length; y++)
                            {   // if at least one of the ray hits then get the aggro                    
                                Debug.DrawLine(controller.m_EnemyController.raycastEyes[y].position, GMController.instance.playerInfo[controller.m_EnemyController.playerSeenDistance[i].targetIndex].playerController.TargetForEnemies.position, Color.red);
                                if (Physics2D.LinecastNonAlloc(controller.m_EnemyController.raycastEyes[y].position, GMController.instance.playerInfo[controller.m_EnemyController.playerSeenDistance[i].targetIndex].playerController.TargetForEnemies.position, controller.m_EnemyController.lineCastHits, controller.enemyStats.obstacleMask) <= 0)
                                {
                                    controller.m_EnemyController.playerSeen = true;
                                    controller.m_EnemyController.startSwoopPosition = controller.m_EnemyController.thisTransform.position;
                                    controller.m_EnemyController.endSwoopPosition = GMController.instance.playerInfo[controller.m_EnemyController.playerSeenDistance[i].targetIndex].player.transform.position;
                                }
                            }
                        }
                    } 
                    controller.m_EnemyController.currentViewTimer = controller.enemyStats.viewCheckFrequenzy;
                }
            }
        }
    }
}
