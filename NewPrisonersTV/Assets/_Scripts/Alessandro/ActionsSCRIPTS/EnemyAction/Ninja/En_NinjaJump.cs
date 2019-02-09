using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/NinjaJump")] 
    public class En_NinjaJump : _Action
    {
        public override void Execute(EnemiesAIStateController controller)
        {
            Move(controller); 
        }

        public void Move(EnemiesAIStateController controller)
        {
            controller.m_EnemyController.agent.velocity = Vector3.zero;
            controller.m_EnemyController.agent.enabled = false;
            controller.m_EnemyController.rb.velocity = Vector2.zero;
            controller.m_EnemyController.rb.AddRelativeForce((controller.m_EnemyController.thisTransform.forward * controller.enemyStats.ninjaJumpLenght) + (Vector3.up * controller.enemyStats.ninjaJumpHeight), ForceMode2D.Impulse);
            controller.m_EnemyController.currentJumpTimer = controller.enemyStats.ninjaJumpCooldown;
        }
    }
}
