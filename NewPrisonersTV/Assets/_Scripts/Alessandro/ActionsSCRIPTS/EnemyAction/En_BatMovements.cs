using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/BatMovements")]
    public class En_BatMovements : _Action
    {
        public override void Execute(EnemiesAIStateController controller)
        {
            Move(controller);
        }

        public void Move(EnemiesAIStateController controller)
        {
            //sinusoidal movement
            if (!controller.m_EnemyController.playerSeen)
            {
                controller.m_EnemyController.thisTransform.position += new Vector3(controller.enemyStats.speed * controller.m_EnemyController.direction * Time.deltaTime, Mathf.Sin(Time.time * controller.enemyStats.sinusoidalMovement) * Time.deltaTime * controller.enemyStats.sinusoidalMovement, 0);

            }
        }

    }
}
