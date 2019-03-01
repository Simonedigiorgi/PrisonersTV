using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace Character.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Characters/DashResetTimer")]
    public class Ch_DashResetTimer : _Action
    {
        public override void Execute(CharacterStateController controller)
        {
            Timer(controller); 
        }

        public void Timer(CharacterStateController controller)
        {
            controller.m_CharacterController.currentDashTimer = controller.characterStats.dashTimer;                     
        }

    }
}
