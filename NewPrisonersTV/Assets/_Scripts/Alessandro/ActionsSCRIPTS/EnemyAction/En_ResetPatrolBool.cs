using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/ResetPatrolBool")] 
    public class En_ResetPatrolBool : _Action
    {
        public override void Execute(EnemiesAIStateController controller) 
        {
            BoolReset(controller); 
        }

        public void BoolReset(EnemiesAIStateController controller)
        {
            controller.m_EnemyController.firstPatrolSet = false;
        }
    }
}
