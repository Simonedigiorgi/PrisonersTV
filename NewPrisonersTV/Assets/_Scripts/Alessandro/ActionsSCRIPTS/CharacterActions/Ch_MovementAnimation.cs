using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace Character.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Characters/MovementAnim")]
    public class Ch_MovementAnimation : _Action
    {
        public override void Execute(CharacterStateController controller)
        {
            MoveAnim(controller);
        }

        public void MoveAnim(CharacterStateController controller)
        {
            float moveInput;
            // CONTROLLER
            if (GMController.instance.playerInfo[controller.m_CharacterController.playerNumber].ControllerIndex != GMController.instance.KeyboardConfig.ControllerIndex)
            {
                moveInput = Input.GetAxis(controller.m_CharacterController.m_ControlConfig.controller.ToString() +
                                            (GMController.instance.playerInfo[controller.m_CharacterController.playerNumber].ControllerIndex + 1) +
                                            controller.m_CharacterController.m_ControlConfig.LeftHorizontal.ToString());
            }
            // KEYBOARD/MOUSE   
            else
            {
                moveInput = Input.GetAxis(controller.m_CharacterController.m_ControlConfig.controller.ToString() +
                                           controller.m_CharacterController.m_ControlConfig.LeftHorizontal.ToString());
            }
            controller.m_CharacterController.playerAnim.SetFloat("Forward", Mathf.Abs(moveInput));
        }

    }
}
