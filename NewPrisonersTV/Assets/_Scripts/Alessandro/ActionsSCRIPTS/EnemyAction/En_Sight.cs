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

            if (controller.enemyStats.enemyLevel == 3)
            {
                if (!controller.m_EnemyController.playerSeen)
                {
                    for (int i = 0; i < GMController.instance.playerInfo.Length; i++)
                    {       // check distance between player and enemy without Vector2.Distance
                        float rayDistance = (controller.m_EnemyController.thisTransform.position - GMController.instance.playerInfo[i].player.transform.position).sqrMagnitude;
                        if (rayDistance <= (controller.enemyStats.attackView * controller.enemyStats.attackView))
                        {
                            Vector2 rayDirection = GMController.instance.playerInfo[i].player.transform.position - controller.m_EnemyController.thisTransform.position;

                            for (int y = 0; y < controller.m_EnemyController.raycastEyes.Length; y++)
                            {
                                Debug.DrawRay(controller.m_EnemyController.raycastEyes[y].position, rayDirection, Color.red);
                                if (!Physics2D.Raycast(controller.m_EnemyController.raycastEyes[y].position, rayDirection, controller.enemyStats.attackView, controller.enemyStats.obstacleMask))                                   
                                {
                                    controller.m_EnemyController.playerSeen = true;
                                    controller.m_EnemyController.startSwoopPosition = controller.m_EnemyController.thisTransform.position;
                                    controller.m_EnemyController.endSwoopPosition = GMController.instance.playerInfo[i].player.transform.position;
                                }
                            }
                        }                      
                    }
                   
                }
            }
        }

    }
}
