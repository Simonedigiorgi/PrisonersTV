﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using Character;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/KamikazeDeath")]
    public class En_KamikazeDeath : _Action
    {
        public override void Execute(EnemiesAIStateController controller)
        {
            Life(controller);
        }

        public void Life(EnemiesAIStateController controller)
        {
            controller.m_EnemyController.agent.speed = 0;
            //enemy die
            if (controller.m_EnemyController.canExplode || controller.enemyStats.enemyLevel >= 2)
            {
                controller.m_EnemyController.explosionParticle.Explosion(controller.m_EnemyController.thisTransform.position);
            }
            controller.m_EnemyController.startDieCoroutine = true;                             
        }
    }
}