using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/SentinelAttackMovements")]
    public class En_SentinelAttackMovements : _Action
    {
        public override void Execute(EnemiesAIStateController controller)
        {
            Move(controller);
        }

        public void Move(EnemiesAIStateController controller)
        {
            if (controller.enemyStats.enemyLevel >= 2)
                controller.m_EnemyController.rb.velocity = new Vector2(controller.enemyStats.speed * controller.m_EnemyController.direction, controller.m_EnemyController.rb.velocity.y);
            else
                controller.m_EnemyController.rb.velocity = Vector2.zero;
        }

    }
}
