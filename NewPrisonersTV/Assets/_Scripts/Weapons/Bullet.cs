using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Bullet : MonoBehaviour {

    [BoxGroup("Controls")] public float speed;                                                          // Bullet speed
    [BoxGroup("Controls")] public float destroyAfter;                                                   // Destroy the bullet after seconds

    protected int damage;
    protected bool destroyOnEnemyCollision;
    public bool canBounce;

    //public bool bounce = false;   //Bounce Test
    //protected Vector3 bounceDir;  //Bounce Test

    [HideInInspector]
    public int membership;                                                                              // shoted from player1 or player2

    private void OnEnable()
    {
        // Autodestroy the bullet
        Destroy(gameObject, destroyAfter);
    }
    // Bounce Direction Test
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") && canBounce)
        {
            onWallHit(collision);
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Destroy at impact || destroy after seconds
        if(!canBounce)
        onWallHit(collision);

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("You hit the Enemy");

            //Substract enemy life
            Enemy enemyHit = collision.gameObject.GetComponent<Enemy>();
            enemyHit.enemyMembership = membership;
            enemyHit.life -= damage;

            if (destroyOnEnemyCollision)
                gameObject.SetActive(false);
        }

        if (collision.gameObject.CompareTag("Player_2"))
        {
            Debug.Log("You hit Player 2");
        }

        if (collision.gameObject.CompareTag("Player_1"))
        {
            Debug.Log("You hit Player 1");
        }
    }

    protected virtual void onWallHit(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Bullet hit the wall");
            Destroy(gameObject);
        }
    }

    protected virtual void onWallHit(Collision2D collision)
    {
       
    }
}
