using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace Character.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Characters/TensionTimers")]
    public class Ch_TensionTimers : _Action
    {
        public override void Execute(CharacterStateController controller)
        {
            Timers(controller); 
        }

        public void Timers(CharacterStateController controller)
        {
            if (controller.m_CharacterController.tensionUpTimer > 0 && Mathf.Abs(controller.m_CharacterController.moveInput) >= Mathf.Abs(controller.m_CharacterController.m_CharStats.joypadDeathZone))
                controller.m_CharacterController.tensionUpTimer -= Time.deltaTime;
            else if (controller.m_CharacterController.tensionUpTimer <= 0)
            {
                controller.m_CharacterController.tensionUpTimer = GMController.instance.tensionStats.movementTimer;
                GMController.instance.TensionThresholdCheck(GMController.instance.tensionStats.movementPoints); // add tension points for action
            } 

            if (controller.m_CharacterController.tensionDownTimer > 0 && Mathf.Abs(controller.m_CharacterController.moveInput) < Mathf.Abs(controller.m_CharacterController.m_CharStats.joypadDeathZone))
                controller.m_CharacterController.tensionDownTimer -= Time.deltaTime;
            else if (controller.m_CharacterController.tensionDownTimer <= 0) 
            {
                controller.m_CharacterController.tensionDownTimer = GMController.instance.tensionStats.standStillTimer;
                GMController.instance.LowerTensionCheck(GMController.instance.tensionStats.standStillPoints); // sub tension
            }
        }

    }
}
