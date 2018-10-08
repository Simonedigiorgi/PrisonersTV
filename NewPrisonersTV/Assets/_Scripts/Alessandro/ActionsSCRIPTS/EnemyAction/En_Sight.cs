using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/Sight")]
    public class En_Sight: _Action
    {
        public override void Execute(EnemiesAIStateController controller)
        {
            Sight(controller);
        }

        public void Sight(EnemiesAIStateController controller)
        {

            if (controller.enemyStats.enemyLevel > 1)
            {
                if (!controller.m_EnemyController.playerSeen)
                {
                    for (int i = 0; i < GMController.instance.playerInfo.Length; i++)
                    {
                        if (Vector2.Distance(controller.m_EnemyController.thisTransform.position, GMController.instance.playerInfo[i].player.transform.position) <= controller.enemyStats.attackView)
                        {
                            Vector2 rayDirection = GMController.instance.playerInfo[i].player.transform.position - controller.m_EnemyController.thisTransform.position;
                            Debug.DrawRay(controller.m_EnemyController.thisTransform.position + Vector3.right, rayDirection, Color.red);
                            Debug.DrawRay(controller.m_EnemyController.thisTransform.position + Vector3.left, rayDirection, Color.red);

                            if (!Physics2D.Raycast(controller.m_EnemyController.thisTransform.position + Vector3.right, rayDirection, Vector2.Distance(controller.m_EnemyController.thisTransform.position, GMController.instance.playerInfo[i].player.transform.position), controller.enemyStats.obstacleMask)
                                && !Physics2D.Raycast(controller.m_EnemyController.thisTransform.position + Vector3.left, rayDirection, Vector2.Distance(controller.m_EnemyController.thisTransform.position, GMController.instance.playerInfo[i].player.transform.position), controller.enemyStats.obstacleMask))
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
