using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/BatAttack")]
    public class En_BatAttack : _Action
    {
        public override void Execute(EnemiesAIStateController controller)
        {
            Attack(controller);
        }

        public void Attack(EnemiesAIStateController controller)
        {
            if (controller.enemyStats.enemyLevel == 3 && !controller.m_EnemyController.swoopCoroutineInExecution && controller.m_EnemyController.playerSeen)
            {                                     
                controller.m_EnemyController.startSwoopCR = true;
            } 

        }

    }
}
