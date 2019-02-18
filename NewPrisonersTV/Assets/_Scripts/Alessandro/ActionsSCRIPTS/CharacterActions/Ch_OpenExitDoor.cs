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
            // CONTROLLER
            if (GMController.instance.playerInfo[controller.m_CharacterController.playerNumber].ControllerIndex != GMController.instance.KeyboardConfig.ControllerIndex)
            {
                if (Input.GetButtonDown(controller.m_CharacterController.m_ControlConfig.controller.ToString() +
                                    (GMController.instance.playerInfo[controller.m_CharacterController.playerNumber].ControllerIndex + 1) +
                                    controller.m_CharacterController.m_ControlConfig.interactInput.ToString()) && controller.m_CharacterController.canExit)
                {
                    GMController.instance.gameEnded = true;
                    GMController.instance.gameStart = false;
                    GMController.instance.canResultCR = false;
                    Time.timeScale = 0;
                }
            }
            // KEYBOARD/MOUSE
            else
            {
                if (Input.GetButtonDown(controller.m_CharacterController.m_ControlConfig.controller.ToString() +
                                      controller.m_CharacterController.m_ControlConfig.interactInput.ToString()) && controller.m_CharacterController.canExit)
                {
                    GMController.instance.gameEnded = true;
                    GMController.instance.gameStart = false;
                    GMController.instance.canResultCR = false;
                    Time.timeScale = 0;
                }

            }
        }

    }
}
