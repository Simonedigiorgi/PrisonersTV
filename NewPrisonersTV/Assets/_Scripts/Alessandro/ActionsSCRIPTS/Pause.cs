using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace GM.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/GM/Pause")] 
    public class Pause : _Action
    {

        public override void Execute(GMStateController controller)
        {
            PauseGame(controller);
        }

        private void PauseGame(GMStateController controller)
        {
            if (!GMController.instance.gameEnded && GMController.instance.gameCDEnded) 
            {
                for (int i = 0; i < GMController.instance.playerInfo.Length; i++)
                {
                    if (GMController.instance.gameStart && Input.GetButtonDown(GMController.instance.playerInfo[i].PlayerController.m_ControlConfig.pauseInput.ToString()))
                    {
                        GMController.instance.gameStart = false;                                        // pause the scripts

                        Time.timeScale = 0;
                    }
                    else if (!GMController.instance.gameStart && Input.GetButtonDown(GMController.instance.playerInfo[i].PlayerController.m_ControlConfig.pauseInput.ToString()))
                    {
                        GMController.instance.gameStart = true;

                        Time.timeScale = 1;
                    }
                }
            }
        }
    }
}
