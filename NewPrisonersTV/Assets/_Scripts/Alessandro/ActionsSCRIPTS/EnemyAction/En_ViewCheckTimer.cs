using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/ViewCheckTimer")] 
    public class En_ViewCheckTimer : _Action
    {
        public override void Execute(EnemiesAIStateController controller) 
        {
            Timer(controller); 
        }

        public void Timer(EnemiesAIStateController controller)
        {
            if (controller.m_EnemyController.currentViewTimer > 0)
                controller.m_EnemyController.currentViewTimer -= Time.deltaTime;
        }
    }
}
