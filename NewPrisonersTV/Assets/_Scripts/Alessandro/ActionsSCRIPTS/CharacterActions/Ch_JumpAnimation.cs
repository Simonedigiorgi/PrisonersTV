using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace Character.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Characters/JumpAnim")]
    public class Ch_JumpAnimation : _Action
    {
        public override void Execute(CharacterStateController controller)
        {
            MoveAnim(controller);
        }

        public void MoveAnim(CharacterStateController controller)
        {    
            controller.m_CharacterController.playerAnim.SetBool("Jump", true);           
        }

    }
}
