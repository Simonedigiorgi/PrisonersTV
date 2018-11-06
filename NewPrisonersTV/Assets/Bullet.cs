using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using AI;

public class Bullet : MonoBehaviour
{

    [BoxGroup("Controls")] public float speed;                                                          // Bullet speed
    [BoxGroup("Controls")] public float destroyAfter;                                                   // Destroy the bullet after seconds
    [BoxGroup("Controls")] public bool canBounce;                                                       // Indicates if the bullet can bounce or not
    [BoxGroup("Controls")] public LayerMask obstacleMask;                                               // Obstacle layers

    protected Vector3 dir;
    protected Vector3 newDir;
    protected float colliderBoundX;
    protected int damage;
    protected bool destroyOnEnemyCollision;

    //public bool bounce = false;   //Bounce Test
    //protected Vector3 bounceDir;  //Bounce Test

    [HideInInspector]
    public int membership;                                                                              // shoted from player1 or player2

    private void OnEnable()
    {
        // Autodestroy the bullet
        Destroy(gameObject, destroyAfter);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Destroy at impact || destroy after seconds

        if (collision.gameObject.CompareTag("Enemy"))
        {

            //Substract enemy life
            _EnemyController enemyHit = collision.gameObject.GetComponent<_EnemyController>();
            enemyHit.enemyMembership = membership;
            enemyHit.currentLife -= damage;

            if (destroyOnEnemyCollision)
                gameObject.SetActive(false);
        }


        if (collision.gameObject.CompareTag("Player_1"))
        {
            Debug.Log("You hit Player 1");
        }
    }

    protected virtual void EnvoiromentRaycastCheck()
    {
        Vector2 rayDirection = dir;
        Debug.DrawRay(transform.position + (-transform.right * colliderBoundX / 2), rayDirection, Color.red);


        RaycastHit2D hit = Physics2D.Raycast(transform.position + (-transform.right * colliderBoundX / 2), rayDirection, 1.0f, obstacleMask);
        if (hit && !canBounce)
        {
            Destroy(gameObject);
        }
    }
}
