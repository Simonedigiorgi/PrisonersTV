using UnityEngine;
using System.Collections;

public class RotateTriggers : MonoBehaviour
{
    public float rotateDegreesPerSecond = 180.0f;
    private Vector3 direction = Vector3.zero;
    public float moveSpeed = 10.0f;
    public float gravity = 10.0f;


    void Update()
    {
        /*float cordX = Input.GetAxis("Horizontal");
        float cordZ = Input.GetAxis("Vertical");

        // If direction is not zero, rotate towards direction (via rotateDegreesPerSecond) and move CharacterController in current forward vector
        if (direction != Vector3.zero)
        {
            // Take care of direction being directly behind our current forward vector since the 
            // Quaternion.RotateTowards will continuously alternate between right and left rotations to get there,
            // resulting in never getting there, just wiggling around the current transform.rotation.
            if (Vector3.Angle(transform.forward, direction) > 179)
            {
                // This will cause us to always turn to the right to go the opposite direction
                direction = transform.TransformDirection(new Vector3(.01f, 0, -1));
            }
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), rotateDegreesPerSecond * Time.deltaTime);

            Vector3 moveDirection = transform.forward;
            moveDirection.y -= gravity * Time.deltaTime;
        }*/
    


        /*float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float joystickMagnitude = Mathf.Clamp01(new Vector2(horizontal, vertical).magnitude);
        float angle = Mathf.Atan2(horizontal, vertical) * Mathf.Rad2Deg;

        Vector3 movement = new Vector3(0, Mathf.Atan2(horizontal, vertical), 0);*/

        /*float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0);
        transform.rotation = Quaternion.LookRotation(movement);*/

        /*Vector3 NextDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (NextDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(NextDir);
        }*/

        //transform.right = Vector3.Normalize(new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")));

        /*float faceDirection = Input.GetAxisRaw("Horizontal");
        if (faceDirection != 0)
        {
            transform.forward = new Vector3(faceDirection, 0, 0);
        }*/

        /*Vector3 shootDirection = Vector3.up * Input.GetAxis("Horizontal") + Vector3.right * Input.GetAxis("Vertical") *- 1;
        if (shootDirection.sqrMagnitude > 0.0f)
        {
            transform.rotation = Quaternion.LookRotation(shootDirection, Vector3.down);
        }*/

        //transform.up = Vector3.Normalize(new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")));

        /*float translate = Input.GetAxis("Horizontal");
        transform.forward = new Vector3(translate, 0, 0);
        transform.Rotate(Vector3.forward * Mathf.Abs(translate) * 5 * Time.deltaTime);*/

        /*float h = Input.GetAxis("Horizontal");//thies work with WASD or arrow keys + other things unity can use, gamepad etc..
        float v = Input.GetAxis("Vertical");
        if (v != 0 || h != 0)
        {// if we are getting inputs, form -1 to 1, 0=no input
            Vector3 direction = (Vector3.forward * h) + (Vector3.right * v);// as the key is held down the h or v values will move toward full -1 or 1, thus handling the smooth rotation
            transform.rotation = Quaternion.LookRotation(Vector3.forward ,direction);
        }*/

        /*Vector3 a = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 b = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 direction = a - b;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;*/


    }
}