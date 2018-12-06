using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/DogDisengageTimerReser")] 
    public class En_DogDisengageTimerReser : _Action
    {
        public override void Execute(EnemiesAIStateController controller) 
        {
            Timer(controller); 
        }

        public void Timer(EnemiesAIStateController controller)
        {          
            controller.m_EnemyController.currentDisengageTimer = controller.enemyStats.disengageTimer;             
        }
    }
}
