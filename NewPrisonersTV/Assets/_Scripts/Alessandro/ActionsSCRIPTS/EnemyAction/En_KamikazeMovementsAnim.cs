using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/KamikazeMovementsAnim")]
    public class En_KamikazeMovementsAnim : _Action
    {
        public override void Execute(EnemiesAIStateController controller)
        {
            MoveAnim(controller);
        }

        public void MoveAnim(EnemiesAIStateController controller)
        { 
            if (!controller.m_EnemyController.playerSeen)
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
                //---------------------------------------------------------------------------------------

                //Vector3 worldDeltaPosition = controller.m_EnemyController.agent.nextPosition - controller.m_EnemyController.thisTransform.position;

                //// Map 'worldDeltaPosition' to local space
                //float dx = Vector3.Dot(controller.m_EnemyController.thisTransform.right, worldDeltaPosition);
                //float dz = Vector3.Dot(controller.m_EnemyController.thisTransform.forward, worldDeltaPosition);
                //Vector3 deltaPosition = new Vector3(dx, controller.m_EnemyController.thisTransform.position.y, dz);

                //// Low-pass filter the deltaMove
                //float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
                //controller.m_EnemyController.smoothDeltaPosition = Vector3.Lerp(controller.m_EnemyController.smoothDeltaPosition, deltaPosition, smooth);

                //// Update velocity if time advances
                //if (Time.deltaTime > 1e-5f)
                //    controller.m_EnemyController.velocity = controller.m_EnemyController.smoothDeltaPosition / Time.deltaTime;

                //controller.m_EnemyController.enemyAnim.SetFloat("Forward", Mathf.Clamp((controller.m_EnemyController.velocity.magnitude), 0, 0.5f));
                //controller.m_EnemyController.agent.speed = (controller.m_EnemyController.enemyAnim.deltaPosition / Time.deltaTime).magnitude;
            }
        }

    }
}
