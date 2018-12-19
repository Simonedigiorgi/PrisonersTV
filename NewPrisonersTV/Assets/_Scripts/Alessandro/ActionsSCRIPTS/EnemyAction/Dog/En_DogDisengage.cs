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
                if (rayDistance <= (controller.enemyStats.chasingView * controller.enemyStats.chasingView) && GMController.instance.playerInfo[controller.m_EnemyController.playerSeenIndex].playerController.isAlive)
                {
                    controller.m_EnemyController.numRayHitPlayer = 0;                     
                    for (int y = 0; y < controller.m_EnemyController.raycastEyes.Length; y++)
                    {
                        Debug.DrawLine(controller.m_EnemyController.raycastEyes[y].position, GMController.instance.playerInfo[controller.m_EnemyController.playerSeenIndex].playerController.TargetForEnemies.position, Color.red);
                        if (Physics2D.LinecastNonAlloc(controller.m_EnemyController.raycastEyes[y].position, GMController.instance.playerInfo[controller.m_EnemyController.playerSeenIndex].playerController.TargetForEnemies.position, controller.m_EnemyController.lineCastHits, controller.enemyStats.obstacleMask) <= 0)
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
                if (controller.m_EnemyController.currentPathTimer <= 0)
                {   // assign path to agent and calculate path to player   
                    controller.m_EnemyController.agent.destination = GMController.instance.playerInfo[controller.m_EnemyController.playerSeenIndex].player.transform.position;
                    controller.m_EnemyController.agent.CalculatePath(controller.m_EnemyController.agent.destination, controller.m_EnemyController.path);
                    
                    // check if the target is still alive
                    if(!GMController.instance.playerInfo[controller.m_EnemyController.playerSeenIndex].playerController.isAlive)
                        controller.m_EnemyController.playerSeen = false;
                    // reset path timer
                    controller.m_EnemyController.currentPathTimer = controller.enemyStats.pathCheckFrequenzy;
                }
                // if the player is unreachable it starts the disengage countdown
                if (controller.m_EnemyController.path.status == UnityEngine.AI.NavMeshPathStatus.PathPartial)
                {
                    if (controller.m_EnemyController.currentDisengageTimer >= 0)
                        controller.m_EnemyController.currentDisengageTimer -= Time.deltaTime;
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
