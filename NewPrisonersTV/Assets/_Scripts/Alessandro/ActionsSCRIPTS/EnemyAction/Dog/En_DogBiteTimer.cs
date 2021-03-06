﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/DogBiteTimer")] 
    public class En_DogBiteTimer : _Action
    {
        public override void Execute(EnemiesAIStateController controller) 
        {
            Timer(controller); 
        }

        public void Timer(EnemiesAIStateController controller)
        {
            if (controller.m_EnemyController.currentBiteTimer > 0)
            {
                controller.m_EnemyController.currentBiteTimer -= Time.deltaTime;
            }
        }
    }
}
