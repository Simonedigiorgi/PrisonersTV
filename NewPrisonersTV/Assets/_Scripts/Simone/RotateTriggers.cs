using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RotateTriggers : MonoBehaviour
{
    Text x;
    Text y;
    PlayerController pc;

    public void Start()
    {
        pc = GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        JoyRotation();
        //MouseRotation();
    }

    /*public void MouseRotation()
    {
        Vector3 mouse_pos = Input.mousePosition;
        Vector3 player_pos = Camera.main.WorldToScreenPoint(this.transform.position);

        mouse_pos.x = mouse_pos.x - player_pos.x;
        mouse_pos.y = mouse_pos.y - player_pos.y;

        float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }*/

    public void JoyRotation()
    {
        Vector3 joyPosition = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        float angle = Mathf.Atan2(joyPosition.y, joyPosition.x) * Mathf.Rad2Deg;

        if (angle == 0 && pc.facingRight)
            angle = 180;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}