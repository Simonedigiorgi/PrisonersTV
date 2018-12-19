using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/SightSpiderAttack")]
    public class En_SightSpiderAttack: _Action
    { 
        public override void Execute(EnemiesAIStateController controller) 
        {
            Sight(controller);
        }

        public void Sight(EnemiesAIStateController controller)
        {
            if (controller.m_EnemyController.currentViewTimer <= 0)
            { // check distance between target and enemy without Vector2.Distance
                float rayDistance = (controller.m_EnemyController.thisTransform.position - GMController.instance.playerInfo[controller.m_EnemyController.playerSeenIndex].playerController.TargetForEnemies.position).sqrMagnitude;
                if (rayDistance <= (controller.enemyStats.chasingView * controller.enemyStats.chasingView) && GMController.instance.playerInfo[controller.m_EnemyController.playerSeenIndex].playerController.isAlive)
                {
                    controller.m_EnemyController.numRayHitPlayer = 0;                  
                    for (int y = 0; y < controller.m_EnemyController.raycastEyes.Length; y++)
                    {
                        Debug.DrawLine(controller.m_EnemyController.raycastEyes[y].position, GMController.instance.playerInfo[controller.m_EnemyController.playerSeenIndex].playerController.TargetForEnemies.position, Color.red);                  
                        if(Physics2D.LinecastNonAlloc(controller.m_EnemyController.raycastEyes[y].position, GMController.instance.playerInfo[controller.m_EnemyController.playerSeenIndex].playerController.TargetForEnemies.position,controller.m_EnemyController.lineCastHits ,controller.enemyStats.obstacleMask) <= 0)
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

    

