using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bat : Enemy
{
    private int direction;

    [Tooltip("Amplitude of sinusoidal movement")] public int sinusoidalMovement;

    [Tooltip("Initial movement directions")] public startDirectin myStartDirection;

    [Tooltip("View distance")] public sbyte attackView;

    [Tooltip("Bat view obstacle")]public LayerMask obstacleMask;

    bool player1Seen = false;
    bool player2Seen = false;

    bool swoopCoroutineInExecution = false;

    Vector3 startSwoopPosition;
    Vector3 endSwoopPosition;

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

        #region BatView

        if(enemyLevel > 1)
        {
            if(!player2Seen && !player1Seen)
            {
                Debug.Log(Vector2.Distance(transform.position, player1.transform.position));

                if (player1.activeSelf && Vector2.Distance(transform.position, player1.transform.position) <= attackView)
                {
                    Vector2 rayDirection = player1.transform.position - transform.position;                   
                    /*Debug.DrawRay(transform.position + Vector3.right, rayDirection, Color.red);
                    Debug.DrawRay(transform.position + Vector3.left, rayDirection, Color.red);*/

                    if (!Physics2D.Raycast(transform.position + Vector3.right, rayDirection, Vector2.Distance(transform.position, player1.transform.position), obstacleMask) 
                        && !Physics2D.Raycast(transform.position + Vector3.left, rayDirection, Vector2.Distance(transform.position, player1.transform.position), obstacleMask))
                    {                        
                        player1Seen = true;
                        startSwoopPosition = transform.position;
                        endSwoopPosition = player1.transform.position;
                    }           
                }
                else if(player2.activeSelf && Vector2.Distance(transform.position, player2.transform.position) <= attackView)
                {
                    Vector2 rayDirection = player2.transform.position - transform.position;
                    /*Debug.DrawRay(transform.position + Vector3.right, rayDirection, Color.green);
                    Debug.DrawRay(transform.position + Vector3.left, rayDirection, Color.green);*/

                    if (!Physics2D.Raycast(transform.position + Vector3.right, rayDirection, Vector2.Distance(transform.position, player2.transform.position), obstacleMask)
                        && !Physics2D.Raycast(transform.position + Vector3.left, rayDirection, Vector2.Distance(transform.position, player2.transform.position), obstacleMask))
                    {
                        player2Seen = true;
                        startSwoopPosition = transform.position;
                        endSwoopPosition = player2.transform.position;
                    }
                }
            }
        }

#endregion

        if (enemyLevel == 1)
        {
            //sinusoidal movement
            transform.position += new Vector3(speed * direction * Time.deltaTime, Mathf.Sin(Time.time * sinusoidalMovement) * Time.deltaTime * sinusoidalMovement, 0);
        }
        else if(enemyLevel == 2)
        {
            //if the bat don't see players
            if (!player1Seen && !player2Seen)
            {
                //sinusoidal movement
                transform.position += new Vector3(speed * direction * Time.deltaTime, Mathf.Sin(Time.time * sinusoidalMovement) * Time.deltaTime * sinusoidalMovement, 0);
            }
            else if(!swoopCoroutineInExecution) //start swoop coroutine
            {
                swoopCoroutineInExecution = true;
                StartCoroutine(Swoop());
            }
        }
	}

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //turn the direction if collide on wall
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Enemy"))
        {
            direction *= -1;
        }
    }

    IEnumerator Swoop()
    {
        transform.DOMove(endSwoopPosition, 1.5f, false);
        yield return new WaitUntil(() => transform.position == endSwoopPosition);
        
        transform.DOMove(startSwoopPosition, 1.5f, false);
        yield return new WaitUntil(() => transform.position == startSwoopPosition);

        swoopCoroutineInExecution = false;
        player1Seen = false;
        player2Seen = false;
    }
}
