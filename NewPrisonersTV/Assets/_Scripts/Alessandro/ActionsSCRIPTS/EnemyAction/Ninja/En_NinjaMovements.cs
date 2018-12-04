using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/NinjaMovements")]
    public class En_NinjaMovements : _Action
    {
        public override void Execute(EnemiesAIStateController controller)
        {
            Move(controller);
        }

        public void Move(EnemiesAIStateController controller)
        {
            if (controller.m_EnemyController.agent.isOnOffMeshLink) // on NavMesh Link change the speed to "jump"
                controller.m_EnemyController.agent.speed = controller.enemyStats.jumpSpeed;
            else // if not on NavMesh Link return to normal speed            
                controller.m_EnemyController.agent.speed = controller.enemyStats.speed;                                  
            //---------------------------------------------------------------------------------------       
            // set first destination if needed
            if (controller.m_EnemyController.agent.destination == null)
                controller.m_EnemyController.agent.destination = controller.m_EnemyController.patrolPoints[controller.m_EnemyController.currentDestinationCount].position;
            //---------------------------------------------------------------------------------------
            // check if has reached the next patrol point
            if ((controller.m_EnemyController.thisTransform.position - controller.m_EnemyController.agent.destination).sqrMagnitude
                <= controller.m_EnemyController.agent.stoppingDistance * controller.m_EnemyController.agent.stoppingDistance)
            {
                controller.m_EnemyController.SetNextPatrolPoint();                                     
            }            
        }
    }
}
