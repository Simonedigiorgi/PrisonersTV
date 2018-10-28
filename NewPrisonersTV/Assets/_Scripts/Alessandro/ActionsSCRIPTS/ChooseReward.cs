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
            if (GMController.instance.canChooseReward && GMController.instance.rewardIndex < GMController.instance.DecrescentScoreOrder.Length)
            {
                // give reward in order

                // set the current player controls in the Input Module
                if (!GMController.instance.playerInfo[GMController.instance.DecrescentScoreOrder[GMController.instance.rewardIndex]].playerController.canGetReward)
                {
                    GMController.instance.UI.ChangeInputModule(GMController.instance.playerInfo[GMController.instance.DecrescentScoreOrder[GMController.instance.rewardIndex]].playerController.m_ControlConfig);
                    GMController.instance.playerInfo[GMController.instance.DecrescentScoreOrder[GMController.instance.rewardIndex]].playerController.canGetReward = true;
                }
                        
                // if the players press the interact button then the next one can choose
                if(Input.GetButtonDown(GMController.instance.playerInfo[GMController.instance.DecrescentScoreOrder[GMController.instance.rewardIndex]].playerController.m_ControlConfig.interactInput.ToString()))
                {
                    GMController.instance.lastPlayerThatChooseReward = GMController.instance.DecrescentScoreOrder[GMController.instance.rewardIndex];
                    GMController.instance.rewardIndex++;
                }                  
                
            }
            else if(GMController.instance.canChooseReward && GMController.instance.rewardIndex >= GMController.instance.DecrescentScoreOrder.Length)
            {
                GMController.instance.readyForNextLevel = true;
            }
        }
    }
}
