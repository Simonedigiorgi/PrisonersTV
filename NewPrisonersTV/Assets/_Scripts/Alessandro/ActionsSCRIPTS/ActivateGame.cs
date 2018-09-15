using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace GM.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/GM/ActivateGame")]
    public class ActivateGame : _Action
    {

        public override void Execute(GMStateController controller)
        {
            Activate(controller);
        }

        private void Activate(GMStateController controller)
        {
            controller.m_GM.SetActive(true);
        }
    }
}
