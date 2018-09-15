using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace GM.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/GM/LocalGameStart")]
    public class LocalCoopStart : _Action
    {

        public override void Execute(GMStateController controller)
        {
            SetRules(controller);
        }

        private void SetRules(GMStateController controller)
        {
            //Set variables for this game mode and spawn players
            if (!controller.m_GM.playerSetupDone)
            {
                controller.m_GM.PlayerSetup();
            }
        }
    }
}
