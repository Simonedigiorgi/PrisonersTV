using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace GM.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/GM/ChooseReward")] 
    public class ChooseReward : _Action
    {

        public override void Execute(GMStateController controller)
        {
            Reward(controller);
        }

        private void Reward(GMStateController controller)
        {
            if (GMController.instance.canChooseReward && GMController.instance.rewardIndex < GMController.instance.CrescentScoreOrder.Length)
            {
                // give reward in order

                // set the current player controls in the Input Module
                if (!GMController.instance.playerInfo[GMController.instance.CrescentScoreOrder[GMController.instance.rewardIndex]].PlayerController.canGetReward)
                {
                    GMController.instance.ChangeInputModule(GMController.instance.playerInfo[GMController.instance.CrescentScoreOrder[GMController.instance.rewardIndex]].PlayerController.m_ControlConfig, (GMController.instance.playerInfo[GMController.instance.CrescentScoreOrder[GMController.instance.rewardIndex]].ControllerIndex+1));
                    GMController.instance.playerInfo[GMController.instance.CrescentScoreOrder[GMController.instance.rewardIndex]].PlayerController.canGetReward = true;
                }

                // if the players press the interact button then the next one can choose
                if (Input.GetButtonDown(GMController.instance.playerInfo[GMController.instance.CrescentScoreOrder[GMController.instance.rewardIndex]].PlayerController.InputCompiler(GMController.instance.playerInfo[GMController.instance.CrescentScoreOrder[GMController.instance.rewardIndex]].PlayerController.m_ControlConfig.interactInput.ToString())))
                {
                    GMController.instance.lastPlayerThatChooseReward = GMController.instance.CrescentScoreOrder[GMController.instance.rewardIndex];
                    GMController.instance.UI.RewardSelection();
                    GMController.instance.rewardIndex++;
                }

            }
            else if(GMController.instance.canChooseReward && GMController.instance.rewardIndex >= GMController.instance.CrescentScoreOrder.Length)
            {
                GMController.instance.readyForNextLevel = true;
            }
        }
    }
}
