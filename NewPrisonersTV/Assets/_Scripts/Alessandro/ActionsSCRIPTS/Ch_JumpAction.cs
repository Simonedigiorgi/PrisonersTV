using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace Character.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Characters/JumpAction")]
    public class Ch_JumpAction : _Action
    {
        public override void Execute(CharacterStateController controller)
        {
            Jump(controller);
        }

        public void Jump(CharacterStateController controller)
        {

            // Check groundmask
            controller.m_CharacterController.isGrounded = Physics2D.OverlapCircle(controller.m_CharacterController.groundCheck.transform.position, controller.m_CharacterController.m_CharStats.groundRadius, controller.m_CharacterController.m_CharStats.groundMask);
          
            if (controller.m_CharacterController.isGrounded)
                controller.m_CharacterController.extraJumps = controller.m_CharacterController.m_CharStats.extraJumpValue;


            // Jump Input
            if (Input.GetButtonDown(controller.m_CharacterController.m_ControlConfig.jumpInput.ToString()))
            {
                controller.m_CharacterController.rb.velocity = Vector2.up * controller.m_CharacterController.m_CharStats.jump;
            }
            
        }

    }
}
