using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/SpiderDeathAnim")]
    public class En_SpiderDeathAnim : _Action
    {
        public override void Execute(EnemiesAIStateController controller)
        {
            MoveAnim(controller);
        }

        public void MoveAnim(EnemiesAIStateController controller)
        {
            controller.m_EnemyController.enemyAnim.SetBool("Death", true); 
        }
    }
}
