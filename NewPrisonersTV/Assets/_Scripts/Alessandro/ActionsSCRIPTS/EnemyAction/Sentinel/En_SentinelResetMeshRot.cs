using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/SentinelResetMeshRot")] 
    public class En_SentinelResetMeshRot : _Action
    {
        public override void Execute(EnemiesAIStateController controller) 
        {
            Turn(controller); 
        }

        public void Turn(EnemiesAIStateController controller) 
        {         
            controller.m_EnemyController.thisMesh.localEulerAngles = new Vector3(0, 90, 0);
        }
    }
}
