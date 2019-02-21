using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace Character.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Characters/OpenExitDoor")]
    public class Ch_OpenExitDoor : _Action
    {
        public override void Execute(CharacterStateController controller)
        {
            OpenDoor(controller);
        }

        public void OpenDoor(CharacterStateController controller)
        {
            if (Input.GetButtonDown(controller.m_CharacterController.inputMapping.interactInput)
                                         && controller.m_CharacterController.canExit)
            {
                GMController.instance.gameEnded = true;
                GMController.instance.gameStart = false;
                GMController.instance.canResultCR = false;
                Time.timeScale = 0;
            }
        }

    }
}
