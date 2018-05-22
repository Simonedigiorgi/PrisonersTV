using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private CharacterController controller;
    private Vector2 direction;

    public float speed;
    public float jump;
    public float gravity;

    private bool isDoubleJump;

	void Start () {
        controller = GetComponent<CharacterController>();
	}
	
	void Update () {

        float yStore = direction.y;
        direction = (transform.right * Input.GetAxis("Horizontal"));

        // This normalize the speed if you press two directions at the same time
        direction = direction.normalized * speed;
        direction.y = yStore;

        #region Jump && Double Jump
        if (controller.isGrounded)
        {
            direction.y = 0f;

            if (Input.GetButtonDown("Fire1"))
            {
                direction.y = jump;
                isDoubleJump = true;
            }
        }
        else if (isDoubleJump && controller.isGrounded == false)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                direction.y = jump;
                isDoubleJump = false;
            }
        }
        #endregion

        // To change the Psysics Gravity go to Edit/Project Settings/Physics
        direction.y = direction.y + (Physics.gravity.y * gravity * Time.deltaTime);
        controller.Move(direction * Time.deltaTime);
    }
}
