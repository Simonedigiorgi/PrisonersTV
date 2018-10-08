using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace GM.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/GM/SurvivalStart")]
    public class SurvivalStart : _Action
    {

        public override void Execute(GMStateController controller)
        {
            SetRules(controller);
        }

        private void SetRules(GMStateController controller)
        {
 
           
        }
    }
}
