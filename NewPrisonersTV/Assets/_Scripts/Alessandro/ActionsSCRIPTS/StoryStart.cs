using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace GM.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/GM/StoryStart")]
    public class StoryStart : _Action
    {

        public override void Execute(GMStateController controller)
        {
            SetRules(controller);
        }

        private void SetRules(GMStateController controller)
        {
            Time.timeScale = 1;
            GMController.instance.currentGameTime = GMController.instance.gameTimer;
            GMController.instance.canStartGameCD = true;
        }
    }
}
