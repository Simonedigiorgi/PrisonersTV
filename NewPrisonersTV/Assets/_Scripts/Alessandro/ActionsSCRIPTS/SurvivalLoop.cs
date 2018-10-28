using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace GM.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/GM/SurvivalLoop")]
    public class SurvivalLoop : _Action
    {

        public override void Execute(GMStateController controller)
        {
            CheckGame(controller);
        }

        private void CheckGame(GMStateController controller)
        {
            //update points, check player and enemies state and respawn

            for (int i = 0; i < controller.m_GM.playerInfo.Length; i++)
            {
                if (controller.m_GM.playerInfo[i].playerController.isAlive)
                {
                    // Player life
                    if (controller.m_GM.playerInfo[i].playerController.currentLife <= 0)
                    {
                        controller.m_GM.playerInfo[i].playerController.currentLife = 0;
                        controller.m_GM.playerInfo[i].playerController.isAlive = false;
                        GMController.instance.UI.SetContinueText(i); // set continue text if needed
                    }
                }

            }

        }
    }
}
