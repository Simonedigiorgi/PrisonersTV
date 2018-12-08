using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/SentinelAttack")] 
    public class En_SentinelAttack : _Action
    {
        public override void Execute(EnemiesAIStateController controller)
        {
            Attack(controller);
        }

        public void Attack(EnemiesAIStateController controller)
        {
            if (controller.enemyStats.enemyLevel < 3 && controller.m_EnemyController.currentBulletTimer <= 0)
            {
                controller.m_EnemyController.bullet.EmitBullet(controller.m_EnemyController.attackSpawn, controller.m_EnemyController.thisMesh.forward);
                controller.m_EnemyController.currentBulletTimer = controller.enemyStats.bulletCooldown;
            }
            else if(controller.enemyStats.enemyLevel == 3 && controller.m_EnemyController.currentBulletTimer <= 0 && controller.m_EnemyController.isBarrageDone)
            {
                controller.m_EnemyController.canBarrageCR = true;
            }         
        }
    }
}
