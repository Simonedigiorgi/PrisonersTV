using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace Character.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Characters/DeathStartAction")]
    public class Ch_DeathStartAction : _Action
    {
        public override void Execute(CharacterStateController controller)
        {
            Death(controller);
        }

        public void Death(CharacterStateController controller)
        {
            controller.m_CharacterController.startDeathCR = true;
        }

    }
}
