using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy
{
    private int direction;

    [Tooltip("Initial movement directions")] public startDirectin myStartDirection;

	protected override void Start ()
    {
        base.Start();

        //assign start directions
		if (myStartDirection == startDirectin.right)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }
	}

    protected override void Update ()
    {
        base.Update();

        //move bat
        transform.position += Vector3.right * speed * direction * Time.deltaTime;
	}

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //turn the direction if collide on wall
        if (collision.gameObject.CompareTag("Wall"))
        {
            direction *= -1;
        }
    }
}
