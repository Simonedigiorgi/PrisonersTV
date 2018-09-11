using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ShootController3D : MonoBehaviour
{
    PlayerController3D player;                                                                                    // Get PlayerController script

    [BoxGroup("Player Inputs")] public BUTTONS shootInput;                                                       // Player1_Button X || Player2_Button X (Shoot with you weapon)

    void Awake()
    {
        player = GetComponent<PlayerController3D>();
    }

    void Update()
    {
        // IF YOU GOT THE WEAPON
        if (player.playerArm.transform.GetChild(0).childCount == 1)
        {
            // Get the weapon component
            Weapon3D weapon = player.playerArm.transform.GetChild(0).GetChild(0).GetComponent<Weapon3D>();

            if (player.isActive)
            {
                // Enable The rotation of joystick
                player.JoyRotation();

                // Shoot condition
                if (weapon.autoFire == false)
                {
                    if (Input.GetButtonDown(shootInput.ToString()) && weapon.isGrabbed)
                    {
                        weapon.Shoot();

                        CameraShake shake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
                        shake.ShakeCamera(1.2f, .2f);
                    }
                }
                if (weapon.autoFire)
                {
                    if (Input.GetButton(shootInput.ToString()) && weapon.isGrabbed)

                        weapon.Shoot();

                        CameraShake shake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
                        shake.ShakeCamera(1.2f, .2f);
                }

                // Flip the weapon when equipped
                if (player.facingRight && weapon.isGrabbed)
                    weapon.transform.localEulerAngles = new Vector3(180, 0, 0);
                else if (player.facingRight == false && weapon.isGrabbed)
                    weapon.transform.localEulerAngles = new Vector3(0, 0, 0);

                // Rotation of Muzz effect
                MuzzRotation();
                return;
            }
        }
    }

    // Muzz flash rotation
    public void MuzzRotation()
    {
        // Get the Muzz component on weapon
        Transform muzzObject = player.playerArm.transform.GetChild(0).GetChild(0).GetChild(2);

        Vector3 muzzPosition = new Vector3(Input.GetAxis(player.Horizontal.ToString()), Input.GetAxis(player.Vertical.ToString()), 0);

        float angle = Mathf.Atan2(muzzPosition.y, muzzPosition.x) * Mathf.Rad2Deg;

        if (angle == 0 && player.facingRight)
            angle = 180;

        muzzObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // This avoid the muzz having a glitchy position
        if (player.facingRight)
            muzzObject.GetComponent<SpriteRenderer>().flipY = true;
        else
            muzzObject.GetComponent<SpriteRenderer>().flipY = false;
    }
}
