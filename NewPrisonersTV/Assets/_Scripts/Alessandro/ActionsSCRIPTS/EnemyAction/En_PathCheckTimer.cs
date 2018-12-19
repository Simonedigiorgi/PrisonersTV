using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/PathCheckTimer")] 
    public class En_PathCheckTimer : _Action
    {
        public override void Execute(EnemiesAIStateController controller) 
        {
            Timer(controller); 
        }

        public void Timer(EnemiesAIStateController controller)
        {
            if (controller.m_EnemyController.currentPathTimer > 0)
                controller.m_EnemyController.currentPathTimer -= Time.deltaTime;
        }
    }
}
