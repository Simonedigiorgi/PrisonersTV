using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/SentinelGroundCheck")]
    public class En_SentinelGoundCheck : _Action
    {
        public override void Execute(EnemiesAIStateController controller)
        {
            MoveCheck(controller);
        }

        public void MoveCheck(EnemiesAIStateController controller) 
        {
            RaycastHit2D groundCheck = Physics2D.Raycast(controller.m_EnemyController.checkPosition.position ,Vector2.down, controller.enemyStats.groundRayDistance, controller.enemyStats.obstacleMask);
            Debug.DrawRay(controller.m_EnemyController.checkPosition.position, Vector2.down,Color.red);  
            if(!groundCheck)
            {
                controller.m_EnemyController.direction *= -1;
                controller.m_EnemyController.RotateTowardDirection(controller.m_EnemyController.thisTransform, controller.m_EnemyController.direction);  
            }
        }

    }
}
