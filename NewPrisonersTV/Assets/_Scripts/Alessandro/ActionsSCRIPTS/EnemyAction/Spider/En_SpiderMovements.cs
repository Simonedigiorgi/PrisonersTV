using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/SpiderMovements")]
    public class En_SpiderMovements : _Action
    {
        public override void Execute(EnemiesAIStateController controller)
        {
            Move(controller);
        }

        public void Move(EnemiesAIStateController controller) 
        {
            if (!controller.m_EnemyController.mine.isActive)// activate mines
            {
                //drop mines
                controller.m_EnemyController.mine.isActive = true;
            }
            //---------------------------------------------------------------------------------------
            if (controller.m_EnemyController.agent.isOnOffMeshLink)
                controller.m_EnemyController.agent.speed = controller.enemyStats.jumpSpeed;
            else
                controller.m_EnemyController.agent.speed = controller.enemyStats.speed;
            //---------------------------------------------------------------------------------------
            if (!controller.m_EnemyController.firstPatrolSet && controller.m_EnemyController.agent.destination != controller.m_EnemyController.patrolPoints[controller.m_EnemyController.currentDestinationCount].position)
            {
                controller.m_EnemyController.agent.destination = controller.m_EnemyController.patrolPoints[controller.m_EnemyController.currentDestinationCount].position;
                controller.m_EnemyController.firstPatrolSet = true;
                //Debug.Log(controller.m_EnemyController.agent.destination);
            }
            //---------------------------------------------------------------------------------------
            // check if has reached the next patrol point           
            if (controller.m_EnemyController.firstPatrolSet && (controller.m_EnemyController.thisTransform.position - controller.m_EnemyController.agent.destination).sqrMagnitude
                <= controller.m_EnemyController.agent.stoppingDistance * controller.m_EnemyController.agent.stoppingDistance)
            {
                //Debug.Log("Next"); 
                controller.m_EnemyController.SetNextPatrolPoint();                                     
            }            
        }
    }
}
