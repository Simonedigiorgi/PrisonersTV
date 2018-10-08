using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace Character.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Characters/ShootAction")]
    public class Ch_ShootAction : _Action
    {
        public override void Execute(CharacterStateController controller)
        {
            Shoot(controller);
        }

        public void Shoot(CharacterStateController controller)
        {
            // IF YOU GOT THE WEAPON
            if (controller.m_CharacterController.currentWeapon != null)
            {
                //if(controller.m_CharacterController.facingRight)
                //{
                    //controller.m_CharacterController.SwapArm(controller.m_CharacterController.facingRight);
                    controller.m_CharacterController.WeaponControl(controller.m_CharacterController.playerRightArm);
                //}
                //else if(!controller.m_CharacterController.facingRight)
                //{
                //    controller.m_CharacterController.SwapArm(controller.m_CharacterController.facingRight);
                //    controller.m_CharacterController.WeaponControl(controller.m_CharacterController.playerLeftArm);
                //}
            }
            else
            { // Disable arm layer 
                controller.m_CharacterController.playerAnim.SetLayerWeight(1, 0);
            }                  
        }
    }
}
