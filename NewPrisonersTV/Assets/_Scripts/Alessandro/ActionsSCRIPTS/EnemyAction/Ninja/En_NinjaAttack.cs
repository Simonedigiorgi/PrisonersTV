using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/NinjaAttack")]
    public class En_NinjaAttack: _Action
    {
        public override void Execute(EnemiesAIStateController controller) 
        {
            Sight(controller);
        }

        public void Sight(EnemiesAIStateController controller)
        {
            if (controller.m_EnemyController.playerSeen && controller.m_EnemyController.currentShurikenTimer <= 0)
            {
                controller.m_EnemyController.shuriken.EmitBullet(controller.m_EnemyController.shurikenSpawn, controller.m_EnemyController.playerSeenIndex);
                controller.m_EnemyController.currentShurikenTimer = controller.enemyStats.ShurikenCooldown;
                controller.m_EnemyController.playerSeen = false;
            } 
        }
    }

    
}
