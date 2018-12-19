using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/SightSentinelAttack")]
    public class En_SightSentinelAttack: _Action
    { 
        public override void Execute(EnemiesAIStateController controller) 
        {
            Sight(controller);
        }

        public void Sight(EnemiesAIStateController controller)
        {
            if (controller.m_EnemyController.currentViewTimer <= 0)
            { 
                float rayDistance = (controller.m_EnemyController.thisTransform.position - GMController.instance.playerInfo[controller.m_EnemyController.playerSeenIndex].playerController.TargetForEnemies.position).sqrMagnitude;
                if (rayDistance <= (controller.enemyStats.attackView * controller.enemyStats.attackView) && GMController.instance.playerInfo[controller.m_EnemyController.playerSeenIndex].playerController.isAlive)
                {
                    controller.m_EnemyController.numRayHitPlayer = 0;
                    Vector2 rayDirection = GMController.instance.playerInfo[controller.m_EnemyController.playerSeenIndex].playerController.TargetForEnemies.position - controller.m_EnemyController.thisTransform.position;
                    for (int y = 0; y < controller.m_EnemyController.raycastEyes.Length; y++)
                    {
                        Debug.DrawRay(controller.m_EnemyController.raycastEyes[y].position, rayDirection, Color.red);
                        if (!Physics2D.Raycast(controller.m_EnemyController.raycastEyes[y].position, rayDirection, rayDistance/rayDistance, controller.enemyStats.obstacleMask))
                        {
                            controller.m_EnemyController.numRayHitPlayer++;
                        }
                    }

                    if (controller.m_EnemyController.numRayHitPlayer == 0)
                    {
                        controller.m_EnemyController.playerSeen = false;
                    }
                }
                else
                {
                    controller.m_EnemyController.playerSeen = false;
                }
                controller.m_EnemyController.currentViewTimer = controller.enemyStats.viewCheckFrequenzy;
            }
        }
    }
}

    

