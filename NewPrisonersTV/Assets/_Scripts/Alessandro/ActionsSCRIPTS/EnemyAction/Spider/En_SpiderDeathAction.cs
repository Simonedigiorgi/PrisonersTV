using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/SpiderDeathAction")]
    public class En_SpiderDeathAction : _Action
    {
        public override void Execute(EnemiesAIStateController controller)
        {
            MoveAnim(controller);
        }

        public void MoveAnim(EnemiesAIStateController controller)
        {
            controller.m_EnemyController.agent.isStopped = true;
            if(controller.enemyStats.enemyLevel == 3)
            {
                controller.m_EnemyController.mine.MineGrapple();
            }
            controller.m_EnemyController.startDieCoroutine = true;
        }
    }
}
