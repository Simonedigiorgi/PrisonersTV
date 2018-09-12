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
            if (controller.m_CharacterController.playerArm.transform.GetChild(0).childCount == 1)
            {
                // Get the weapon component
                if (controller.m_CharacterController.currentWeapon == null)
                {
                    controller.m_CharacterController.currentWeapon = controller.m_CharacterController.playerArm.transform.GetChild(0).GetChild(0).GetComponent<Weapon3D>();
                }

                if (controller.m_CharacterController.isActive)
                {
                    // Enable The rotation of joystick
                    if (controller.m_CharacterController.moveArmWithRight)
                    {
                        Debug.Log("right");
                        controller.m_CharacterController.JoyRotation(controller.m_CharacterController.RightHorizontal, controller.m_CharacterController.RightVertical);
                    }
                    else if (!controller.m_CharacterController.moveArmWithRight)
                    {
                        Debug.Log("left");
                        controller.m_CharacterController.JoyRotation(controller.m_CharacterController.LeftHorizontal, controller.m_CharacterController.LeftVertical);
                    }
                   
                    // Shoot condition
                    if (controller.m_CharacterController.currentWeapon.autoFire == false)
                    {
                        if (Input.GetButtonDown(controller.m_CharacterController.shootInput.ToString()) && controller.m_CharacterController.currentWeapon.isGrabbed)
                        {
                            controller.m_CharacterController.currentWeapon.Shoot();

                            CameraShake shake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
                            //shake.ShakeCamera(1.2f, .2f);
                        }
                    }
                    if (controller.m_CharacterController.currentWeapon.autoFire)
                    {
                        if (Input.GetButton(controller.m_CharacterController.shootInput.ToString()) && controller.m_CharacterController.currentWeapon.isGrabbed)

                            controller.m_CharacterController.currentWeapon.Shoot();

                        CameraShake shake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
                        shake.ShakeCamera(1.2f, .2f);
                    }

                    // Flip the weapon when equipped
                    if (controller.m_CharacterController.facingRight && controller.m_CharacterController.currentWeapon.isGrabbed)
                        controller.m_CharacterController.currentWeapon.transform.localEulerAngles = new Vector3(180, 0, 0);
                    else if (controller.m_CharacterController.facingRight == false && controller.m_CharacterController.currentWeapon.isGrabbed)
                        controller.m_CharacterController.currentWeapon.transform.localEulerAngles = new Vector3(0, 0, 0);

                    // Rotation of Muzz effect
                    if (controller.m_CharacterController.moveArmWithRight)
                    {
                        controller.m_CharacterController.armRotation(controller.m_CharacterController.RightHorizontal, controller.m_CharacterController.RightVertical);
                    }
                    else if (!controller.m_CharacterController.moveArmWithRight)
                    {
                        controller.m_CharacterController.armRotation(controller.m_CharacterController.LeftHorizontal, controller.m_CharacterController.LeftVertical);
                    }
                    return;
                }
            }
        }

    }
}
