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
            // LATER try to substitute with the move input in the character controller
            float moveInput = Input.GetAxis(controller.m_CharacterController.InputCompiler(controller.m_CharacterController.m_ControlConfig.LeftHorizontal.ToString()));
            controller.m_CharacterController.playerAnim.SetFloat("Forward", Mathf.Abs(moveInput));
        }

    }
}
