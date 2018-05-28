using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Bullet : MonoBehaviour {

    private Rigidbody2D rb;                                                                             // Get the rigidbody2D

    [BoxGroup("Controls")] public float speed;                                                          // Bullet speed
    [BoxGroup("Controls")] public float destroyAfter;                                                   // Destroy the bullet after seconds

    [BoxGroup("Kind of weapon")] public bool isArrow;                                                   // They block on Walls
    [BoxGroup("Kind of weapon")] public bool isLaser;                                                   // Move like a laser beam
    [BoxGroup("Kind of weapon")] public bool isSnake;                                                   // Move as a snake on the walls
    [BoxGroup("Kind of weapon")] public bool isGhost;                                                   // Can surpass walls

    void Start () {

        rb = GetComponent<Rigidbody2D>();

        // Autodestroy the bullet
        Destroy(gameObject, destroyAfter);
    }
	
	void Update () {

        // Direction
        transform.Translate(transform.right * speed * Time.deltaTime, Space.World);

        // Disable collisions
        if(isGhost)
            GetComponent<BoxCollider2D>().enabled = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Destroy at impact || destroy after seconds
        if (collision.gameObject.CompareTag("Wall") && !isArrow)
        {
            if(!isGhost && !isLaser && !isSnake)
                Destroy(gameObject);
            else if (isGhost)
                Destroy(gameObject, destroyAfter);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Arrow behaviour (Stay stuck on wall, you can climb it)
            if (isArrow)
            {
                rb.isKinematic = true;
                speed = 0;
                Destroy(gameObject, destroyAfter);
            }
            // Laser behaviour (Reflect on walls)
            else if (isLaser)
            {
                transform.Rotate(0, 0, -45);
                transform.right = -transform.right;
            }
            // Snake behaviour (Walk on walls)
            else if (isSnake)
            {
                speed = -speed;
                transform.right = -transform.right;
            }
        }

        // Destroy if touch another bullet
        if (collision.gameObject.CompareTag("Bullet"))
            Destroy(gameObject);
    }
}
