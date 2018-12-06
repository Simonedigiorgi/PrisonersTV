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
            if (!controller.m_EnemyController.playerSeen && controller.enemyStats.enemyLevel >= 2)
            {
                for (int i = 0; i < GMController.instance.playerInfo.Length; i++) 
                {       // check distance between players and enemy without Vector2.Distance
                     float rayDistance = (controller.m_EnemyController.thisTransform.position - GMController.instance.playerInfo[i].playerController.TargetForEnemies.position).sqrMagnitude;
                     if (rayDistance <= (controller.enemyStats.attackView * controller.enemyStats.attackView))
                     {
                         Vector2 rayDirection = GMController.instance.playerInfo[i].playerController.TargetForEnemies.position - controller.m_EnemyController.thisTransform.position;
                        
                        for (int y = 0; y < controller.m_EnemyController.raycastEyes.Length; y++)
                        {
                            Debug.DrawRay(controller.m_EnemyController.raycastEyes[y].position, rayDirection, Color.red);
                            if (!Physics2D.Raycast(controller.m_EnemyController.raycastEyes[y].position, rayDirection, controller.enemyStats.attackView, controller.enemyStats.obstacleMask))                          
                            {
                                controller.m_EnemyController.playerSeenIndex = i;
                                controller.m_EnemyController.playerSeen = true;
                            }
                        }
                     } 
                     else
                        controller.m_EnemyController.playerSeen = false; 
                }
                   
            }
        }
    }

    
}
