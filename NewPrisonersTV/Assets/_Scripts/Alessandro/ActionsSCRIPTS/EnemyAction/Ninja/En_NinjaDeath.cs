﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using Character;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/NinjaDeath")]
    public class En_NinjaDeath : _Action
    {
        public override void Execute(EnemiesAIStateController controller)
        {
            Life(controller);
        }

        public void Life(EnemiesAIStateController controller)
        {
            controller.m_EnemyController.agent.speed = 0;
            //enemy die         
            controller.m_EnemyController.startDieCoroutine = true;                              
        }
    }
}