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
            if (controller.m_EnemyController.playerSeen && controller.m_EnemyController.currentBulletTimer <= 0)
            {
                controller.m_EnemyController.bullet.EmitBullet(controller.m_EnemyController.attackSpawn, controller.m_EnemyController.playerSeenIndex);
                controller.m_EnemyController.currentBulletTimer = controller.enemyStats.bulletCooldown;
                controller.m_EnemyController.playerSeen = false;
            } 
            else
                controller.m_EnemyController.playerSeen = false; 
        }
    }

    
}
