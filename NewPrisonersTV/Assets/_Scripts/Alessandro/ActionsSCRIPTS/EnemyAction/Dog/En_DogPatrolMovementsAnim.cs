using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/DogPatrolMovementsAnim")]
    public class En_DogPatrolMovementsAnim : _Action
    {
        public override void Execute(EnemiesAIStateController controller)
        {
            MoveAnim(controller);
        }

        public void MoveAnim(EnemiesAIStateController controller)
        {            
            if (controller.m_EnemyController.agent.isOnOffMeshLink)
                controller.m_EnemyController.enemyAnim.SetBool("Jump", true);
            else
                controller.m_EnemyController.enemyAnim.SetBool("Jump", false);
            //---------------------------------------------------------------------------------------

            Vector3 move = controller.m_EnemyController.agent.velocity;
            if (move.magnitude > 1f) move.Normalize();
            move = controller.m_EnemyController.thisTransform.InverseTransformDirection(move);
            move = Vector3.ProjectOnPlane(move, Vector3.down); 

            float m_ForwardAmount = move.z;
            m_ForwardAmount = Mathf.Clamp(m_ForwardAmount, 0, 0.5f);

            controller.m_EnemyController.enemyAnim.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);               
        }
    }
}
