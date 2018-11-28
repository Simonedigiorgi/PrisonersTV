using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/NinjaShurikenTimer")] 
    public class En_NinjaShurikenTimer : _Action
    {
        public override void Execute(EnemiesAIStateController controller) 
        {
            Timer(controller); 
        }

        public void Timer(EnemiesAIStateController controller)
        {
            if (controller.m_EnemyController.currentShurikenTimer > 0)
            {
                controller.m_EnemyController.currentShurikenTimer -= Time.deltaTime;  
            }
        }
    }
}
