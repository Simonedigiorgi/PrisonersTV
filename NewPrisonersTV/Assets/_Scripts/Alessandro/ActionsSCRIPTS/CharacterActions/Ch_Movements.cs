using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace Character.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Characters/Movements")]
    public class Ch_Movements : _Action
    {
        public override void Execute(CharacterStateController controller)
        {
            Move(controller);
        }

        public void Move(CharacterStateController controller)
        {
            // Move inputs 
            controller.m_CharacterController.moveInput = Input.GetAxis(controller.m_CharacterController.inputMapping.LeftHorizontal);

            // Movements
            if (controller.m_CharacterController.moveInput >= controller.m_CharacterController.m_CharStats.joypadDeathZone)// Move right if "x" axis is over 0.2
            {
                    controller.m_CharacterController.rb.velocity = new Vector2(controller.m_CharacterController.m_CharStats.speed, controller.m_CharacterController.rb.velocity.y);
            }
            else if (controller.m_CharacterController.moveInput <= -controller.m_CharacterController.m_CharStats.joypadDeathZone) // Move left if "x" axis is lower -0.2
            {
                    controller.m_CharacterController.rb.velocity = new Vector2(-controller.m_CharacterController.m_CharStats.speed, controller.m_CharacterController.rb.velocity.y);
            }
            else
            {
                    controller.m_CharacterController.rb.velocity = new Vector2(0, controller.m_CharacterController.rb.velocity.y);
            }

            // Flip the player direction
            if (!controller.m_CharacterController.facingRight && controller.m_CharacterController.moveInput < 0)
                controller.m_CharacterController.PlayerFlip();
            else if (controller.m_CharacterController.facingRight && controller.m_CharacterController.moveInput > 0)
                controller.m_CharacterController.PlayerFlip();           
        }
    }
}
