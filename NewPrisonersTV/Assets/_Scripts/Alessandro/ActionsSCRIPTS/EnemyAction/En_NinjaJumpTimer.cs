using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/NinjaJumpTimer")] 
    public class En_NinjaJumpTimer : _Action
    {
        public override void Execute(EnemiesAIStateController controller)
        {
            Timer(controller); 
        }

        public void Timer(EnemiesAIStateController controller)
        {
            if (controller.m_EnemyController.currentJumpTimer > 0)
            {
                controller.m_EnemyController.currentJumpTimer -= Time.deltaTime;  
            }
        }
    }
}
