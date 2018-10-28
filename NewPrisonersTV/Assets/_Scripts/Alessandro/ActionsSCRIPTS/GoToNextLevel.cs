using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace GM.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/GM/GoToNextLevel")] 
    public class GoToNextLevel : _Action
    {

        public override void Execute(GMStateController controller)
        {
            NextGame(controller);
        }

        private void NextGame(GMStateController controller)
        {
          //for (int i = 0; i < GMController.instance.playerInfo.Length; i++)
          //{
                if (GMController.instance.readyForNextLevel)// && Input.GetButtonDown(GMController.instance.playerInfo[i].playerController.m_ControlConfig.interactInput.ToString()))
                {
                    GMController.instance.NextLevel();
                }
          //}
        }
    }
}
