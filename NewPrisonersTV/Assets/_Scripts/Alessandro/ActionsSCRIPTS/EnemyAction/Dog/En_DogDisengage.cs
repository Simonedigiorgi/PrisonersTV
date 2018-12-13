using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using UnityEditor.AI;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/DogDisengage")] 
    public class En_DogDisengage: _Action
    {
        public override void Execute(EnemiesAIStateController controller) 
        {
            Sight(controller);
        }

        public void Sight(EnemiesAIStateController controller)
        {
            if (controller.enemyStats.enemyLevel <= 2 && controller.m_EnemyController.currentViewTimer <= 0)
            {
                float rayDistance = (controller.m_EnemyController.thisTransform.position - GMController.instance.playerInfo[controller.m_EnemyController.playerSeenIndex].playerController.TargetForEnemies.position).sqrMagnitude;
                if (rayDistance <= (controller.enemyStats.attackView * controller.enemyStats.attackView))
                {
                    controller.m_EnemyController.numRayHitPlayer = 0;
                    Vector2 rayDirection = GMController.instance.playerInfo[controller.m_EnemyController.playerSeenIndex].playerController.TargetForEnemies.position - controller.m_EnemyController.thisTransform.position;
                    for (int y = 0; y < controller.m_EnemyController.raycastEyes.Length; y++)
                    {
                        Debug.DrawRay(controller.m_EnemyController.raycastEyes[y].position, rayDirection, Color.red);
                        if (!Physics2D.Raycast(controller.m_EnemyController.raycastEyes[y].position, rayDirection, controller.enemyStats.attackView, controller.enemyStats.obstacleMask))
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
            else if (controller.enemyStats.enemyLevel == 3)
            {
                if (controller.m_EnemyController.currentViewTimer <= 0)
                {   // calculate path to player 
                    controller.m_EnemyController.agent.CalculatePath(controller.m_EnemyController.agent.destination, controller.m_EnemyController.path);
                    controller.m_EnemyController.currentViewTimer = controller.enemyStats.viewCheckFrequenzy;
                }
                // if the player is unreachable it starts the disengage countdown
                if (controller.m_EnemyController.path.status == UnityEngine.AI.NavMeshPathStatus.PathPartial)
                {
                    Debug.Log("Path not found");
                    if (controller.m_EnemyController.currentDisengageTimer >= 0)
                    {
                        controller.m_EnemyController.currentDisengageTimer -= Time.deltaTime;
                    }
                    else
                        controller.m_EnemyController.playerSeen = false;
                }
                else // reset the timer
                {
                    controller.m_EnemyController.currentDisengageTimer = controller.enemyStats.disengageTimer;
                }
            }             
        }
    }    
}
