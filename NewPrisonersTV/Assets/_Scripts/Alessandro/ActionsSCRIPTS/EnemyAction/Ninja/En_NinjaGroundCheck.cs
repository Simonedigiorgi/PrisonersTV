using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/NinjaGroundCheck")] 
    public class En_NinjaGroundCheck : _Action
    {
        public override void Execute(EnemiesAIStateController controller)
        {
            GroundCheck(controller); 
        }

        public void GroundCheck(EnemiesAIStateController controller)
        {
            controller.m_EnemyController.onGround = Physics2D.OverlapCircle(controller.m_EnemyController.thisTransform.position, controller.enemyStats.groundCheckRadius, controller.enemyStats.obstacleMask);
        }
    }
}
