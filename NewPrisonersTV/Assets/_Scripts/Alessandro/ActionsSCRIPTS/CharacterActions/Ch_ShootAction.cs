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
                controller.m_CharacterController.WeaponControl(controller.m_CharacterController.playerRightArm);       
            }
            else
            { // Disable arm layer 
                controller.m_CharacterController.playerAnim.SetLayerWeight(1, 0);
            }                  
        }
    }
}
