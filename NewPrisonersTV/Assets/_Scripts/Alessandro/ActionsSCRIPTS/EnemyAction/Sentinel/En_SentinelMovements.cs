using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/SentinelMovements")]
    public class En_SentinelMovements : _Action
    {
        public override void Execute(EnemiesAIStateController controller)
        {
            Move(controller);
        }

        public void Move(EnemiesAIStateController controller)
        {          
            controller.m_EnemyController.rb.velocity = new Vector2(controller.enemyStats.speed * controller.m_EnemyController.direction, controller.m_EnemyController.rb.velocity.y); 
        }

    }
}
