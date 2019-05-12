using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/KamikazeAttackTorsoAnim")]
    public class En_KamikazeAttackTorsoAnim : _Action
    {
        public override void Execute(EnemiesAIStateController controller)
        {
            MoveAnimTorso(controller);
        }

        public void MoveAnimTorso(EnemiesAIStateController controller)
        {     
           // controller.m_EnemyController.enemyAnim.SetLayerWeight(1, 1);
            ParticleSystem.EmissionModule emission = controller.m_EnemyController.attackParticle.emission;
            emission.enabled = true;
        }

    }
}
