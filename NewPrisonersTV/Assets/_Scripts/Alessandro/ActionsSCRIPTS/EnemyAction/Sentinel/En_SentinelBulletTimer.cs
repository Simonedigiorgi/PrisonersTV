using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/SentinelBulletTimer")] 
    public class En_SentinelBulletTimer : _Action
    {
        public override void Execute(EnemiesAIStateController controller) 
        {
            Timer(controller); 
        }

        public void Timer(EnemiesAIStateController controller) 
        {
            if (controller.m_EnemyController.currentBulletTimer > 0)
            {
                controller.m_EnemyController.currentBulletTimer -= Time.deltaTime;  
            }
        }
    }
}
