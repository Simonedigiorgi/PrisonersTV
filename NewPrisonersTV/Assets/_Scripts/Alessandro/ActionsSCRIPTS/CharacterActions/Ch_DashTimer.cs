using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace Character.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Characters/DashTimer")]
    public class Ch_DashTimer : _Action
    {
        public override void Execute(CharacterStateController controller)
        {
            Timer(controller); 
        }

        public void Timer(CharacterStateController controller)
        {
            if (controller.m_CharacterController.currentDashTimer > 0)
                controller.m_CharacterController.currentDashTimer -= Time.deltaTime;
            else if (controller.m_CharacterController.currentDashTimer <= 0)
            {
                controller.m_CharacterController.isInDash = false;
                controller.m_CharacterController.playerAnim.SetBool("Dash", false); 
                //controller.m_CharacterController.tensionUpTimer = GMController.instance.tensionStats.movementTimer;
            } 
        
        }

    }
}
