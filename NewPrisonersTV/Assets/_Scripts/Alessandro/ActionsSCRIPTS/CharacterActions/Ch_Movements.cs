﻿using System.Collections;
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
            // *** LEAVE ALL OF THIS ON FIXED UPDATE TO AVOID PROBLEM OCCURING DURING THE FLIP

            // Move inputs
            float moveInput = Input.GetAxis(controller.m_CharacterController.m_ControlConfig.LeftHorizontal.ToString());
            Debug.Log(controller.m_CharacterController.m_ControlConfig.LeftHorizontal.ToString());
            // Movements
            if (moveInput >= controller.m_CharacterController.m_CharStats.joypadDeathZone && !controller.m_CharacterController.isInDash)// Move right if "x" axis is over 0.2
            {
                    controller.m_CharacterController.rb.velocity = new Vector2(controller.m_CharacterController.m_CharStats.speed, controller.m_CharacterController.rb.velocity.y);
            }
            else if (moveInput <= -controller.m_CharacterController.m_CharStats.joypadDeathZone && !controller.m_CharacterController.isInDash) // Move left if "x" axis is lower -0.2
            {
                    controller.m_CharacterController.rb.velocity = new Vector2(-controller.m_CharacterController.m_CharStats.speed, controller.m_CharacterController.rb.velocity.y);
            }
            else if (!controller.m_CharacterController.isInDash)// If is not in Dash
            {
                    controller.m_CharacterController.rb.velocity = new Vector2(0, controller.m_CharacterController.rb.velocity.y);
            }

            // Set Run animation
            //playerAnim.SetFloat("Speed", Mathf.Abs(moveInput));

            // Flip the player direction
            if (!controller.m_CharacterController.facingRight && moveInput < 0)
                controller.m_CharacterController.PlayerFlip();
            else if (controller.m_CharacterController.facingRight && moveInput > 0)
                controller.m_CharacterController.PlayerFlip();
           
        }

    }
}