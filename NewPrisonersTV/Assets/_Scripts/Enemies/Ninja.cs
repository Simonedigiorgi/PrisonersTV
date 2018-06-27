using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Ninja : Enemy
{
    [Tooltip("Time in second needed between one shuriken and other shuriken")] public sbyte ShurikenCooldown;

    private int direction;

    [Tooltip("Initial movement directions")] public startDirectin myStartDirection;

    [Tooltip("View distance")] public sbyte attackView;

    [Tooltip("Ninja view obstacle")] public LayerMask obstacleMask;

    //[BoxGroup("DONT CHANGE")] public LayerMask groundMask;

    Rigidbody2D rb;

    public GameObject Shuriken;
    GameObject groundCheck;

    Vector3 targetPosition;

    bool shurikenCoroutineInExecution = false;

    bool player1Seen = false;
    bool player2Seen = false;
    bool isJumping;
    bool isFacinRight;

    protected override void Start ()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();

        //assign start directions
		if (myStartDirection == startDirectin.right)
        {
            direction = 1;
            isFacinRight = true;
        }
        else
        {
            direction = -1;
            isFacinRight = false;
        }
	}

    protected override void Update ()
    {
        base.Update();

        #region NinjaView

        if(enemyLevel > 1)
        {
            if(!player2Seen && !player1Seen)
            {

                if (player1.activeSelf && Vector2.Distance(transform.position, player1.transform.position) <= attackView)
                {
                    Vector2 rayDirection = player1.transform.position - transform.position;                   
                    /*Debug.DrawRay(transform.position + Vector3.right, rayDirection, Color.red);
                    Debug.DrawRay(transform.position + Vector3.left, rayDirection, Color.red);*/

                    if (!Physics2D.Raycast(transform.position + Vector3.right, rayDirection, Vector2.Distance(transform.position, player1.transform.position), obstacleMask) 
                        && !Physics2D.Raycast(transform.position + Vector3.left, rayDirection, Vector2.Distance(transform.position, player1.transform.position), obstacleMask))
                    {                        
                        targetPosition = player1.transform.position;
                        player1Seen = true;
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
                        targetPosition = player2.transform.position;
                        player2Seen = true;
                    }
                }
            }
        }

        #endregion

        //check ground
        isJumping = !rb.IsTouchingLayers(obstacleMask);

        //move
        transform.position += Vector3.right * speed * direction * Time.deltaTime;

        //flip
        if (direction == 1 && !isFacinRight)
        {
            isFacinRight = true;
            transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
        }
        else if (direction == -1 && isFacinRight)
        {
            isFacinRight = false;
            transform.rotation = Quaternion.Euler(new Vector3(0,180,0));
        }

        if(enemyLevel == 2)
        {
            if(player2Seen || player1Seen)
            {
                if (!shurikenCoroutineInExecution)
                {
                    shurikenCoroutineInExecution = true;
                    StartCoroutine(ShurikenCoroutine());
                }
            }
        }
	}

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //turn the direction if collide on wall
        if (collision.gameObject.CompareTag("Wall") && !isJumping)
        {
            direction *= -1;
        }
    }

    IEnumerator ShurikenCoroutine()
    {
        //take direction of target
        Vector3 direction = targetPosition - transform.position;

        //take angle of shoot
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //instantiate shuriken
        GameObject shuriken = Instantiate(Shuriken, transform.position, Quaternion.identity);

        //rotate shuriken to target
        shuriken.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        yield return new WaitForSeconds(ShurikenCooldown);

        shurikenCoroutineInExecution = false;
        player1Seen = false;
        player2Seen = false;
    }
}
