using UnityEngine;
using System.Collections;

public class RotateTriggers : MonoBehaviour
{
    PlayerController pc;

    public void Start()
    {
        pc = GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        JoyRotation();
    }

    // Rotate the Joystick of 360°
    public void JoyRotation()
    {
        Vector3 joyPosition = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        float angle = Mathf.Atan2(joyPosition.y, joyPosition.x) * Mathf.Rad2Deg;

        if (angle == 0 && pc.facingRight)
            angle = 180;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}