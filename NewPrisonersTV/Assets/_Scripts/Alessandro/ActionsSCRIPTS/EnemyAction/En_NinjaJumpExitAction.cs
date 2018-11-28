using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/NinjaJumpExit")] 
    public class En_NinjaJumpExitAction : _Action
    {
        public override void Execute(EnemiesAIStateController controller) 
        {
            Move(controller);
        }

        public void Move(EnemiesAIStateController controller)
        {
            //controller.m_EnemyController.worldDeltaPosition = controller.m_EnemyController.agent.nextPosition - controller.m_EnemyController.playerMesh.position;
            ////Pull agent towards character 
            //if (controller.m_EnemyController.worldDeltaPosition.magnitude > controller.m_EnemyController.agent.radius) 
            //    controller.m_EnemyController.agent.nextPosition = controller.m_EnemyController.playerMesh.position + 0.9f * controller.m_EnemyController.worldDeltaPosition;                                     
            controller.m_EnemyController.agent.enabled = true; 
        }
    }
}
