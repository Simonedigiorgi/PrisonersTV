using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/SentinelTurnMesh")] 
    public class En_SentinelTurnMesh : _Action
    {
        public override void Execute(EnemiesAIStateController controller) 
        {
            Turn(controller); 
        }

        public void Turn(EnemiesAIStateController controller) 
        {
            Vector3 relativePoint = controller.m_EnemyController.thisTransform.InverseTransformPoint(GMController.instance.playerInfo[controller.m_EnemyController.playerSeenIndex].Player.transform.position);
            if (relativePoint.x < 0.0)
                controller.m_EnemyController.MeshLookAtPlayerDir(-1,90);
            else if (relativePoint.x > 0.0)
                controller.m_EnemyController.MeshLookAtPlayerDir(1,90);          
        }
    }
}
