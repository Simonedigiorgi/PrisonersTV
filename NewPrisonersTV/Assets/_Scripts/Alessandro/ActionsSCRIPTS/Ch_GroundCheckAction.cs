using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace Character.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Characters/GroundCheckAction")]
    public class Ch_GroundCheckAction: _Action
    {
        public override void Execute(CharacterStateController controller)
        {
            GroundCheck(controller);
        }

        public void GroundCheck(CharacterStateController controller)
        {
            // Check groundmask
            controller.m_CharacterController.isGrounded = Physics2D.OverlapCircle(controller.m_CharacterController.groundCheck.transform.position, controller.m_CharacterController.m_CharStats.groundRadius, controller.m_CharacterController.m_CharStats.groundMask);
        }

    }
}
