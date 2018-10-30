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
            if (GMController.instance.readyForNextLevel)
            {
                GMController.instance.NextLevel();
            }
        }
    }
}
