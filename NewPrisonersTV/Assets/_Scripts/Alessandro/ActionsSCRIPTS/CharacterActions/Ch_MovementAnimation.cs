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
            controller.m_CharacterController.playerAnim.SetFloat("Forward", Mathf.Abs(controller.m_CharacterController.moveInput));
        }

    }
}
