using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/LifeCheck")]
    public class En_LifeCheck : _Action
    {
        public override void Execute(EnemiesAIStateController controller)
        {
            Life(controller);
        }

        public void Life(EnemiesAIStateController controller)
        {
            //enemy die
            if (controller.m_EnemyController.currentLife <= 0 && !controller.m_EnemyController.startDieCoroutine)
            {
                controller.m_EnemyController.startDieCoroutine = true;               
            }
        }

    }
}
