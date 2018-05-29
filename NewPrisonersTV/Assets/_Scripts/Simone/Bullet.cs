using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Bullet : MonoBehaviour {

    [BoxGroup("Controls")] public float speed;                                                          // Bullet speed
    [BoxGroup("Controls")] public float destroyAfter;                                                   // Destroy the bullet after seconds

    [BoxGroup("Kind of bullet")] public bool isArrow;                                                   // They block on Walls
    [BoxGroup("Kind of bullet")] public bool isLaser;                                                   // Move like a laser beam
    [BoxGroup("Kind of bullet")] public bool isGhost;                                                   // Can surpass walls

    void Start () {

        // Autodestroy the bullet
        Destroy(gameObject, destroyAfter);
    }
	
	void Update () {

        // Direction
        transform.Translate(transform.right * speed * Time.deltaTime, Space.World);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Destroy if touch another bullet
        if (collision.gameObject.CompareTag("Bullet"))
            Destroy(gameObject);

        // Destroy at impact || destroy after seconds
        if (collision.gameObject.CompareTag("Wall"))
        {
            if (!isArrow)
            {
                if (!isGhost && !isLaser)
                    Destroy(gameObject);
                else if (isGhost)
                    Destroy(gameObject, destroyAfter);
            }

            // Arrow behaviour (Stay stuck on wall)
            else if (isArrow)
            {
                speed = 0;
                Destroy(gameObject, destroyAfter);
            }

            if (isLaser)
            {
                // Laser behaviour (Reflect on walls)
                transform.Rotate(0, 0, -45);
                transform.right = -transform.right;
            }
        }
    }
}
