﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Bullet : MonoBehaviour {

    [BoxGroup("Controls")] public float speed;                                                          // Bullet speed
    [BoxGroup("Controls")] public float destroyAfter;                                                   // Destroy the bullet after seconds

    [BoxGroup("Kind of bullet")] public bool isArrow;                                                   // They block on Walls
    [BoxGroup("Kind of bullet")] public bool isLaser;                                                   // Move like a laser beam
    [BoxGroup("Kind of bullet")] public bool isGhost;                                                   // Can surpass walls

    protected int damage;
    protected bool destroyOnEnemyCollision;

    [HideInInspector]
    public int membership;                                                                              // shoted from player1 or player2

    private void OnEnable()
    {
        // Autodestroy the bullet
        Destroy(gameObject, destroyAfter);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Destroy if touch another bullet
        if (collision.gameObject.CompareTag("Bullet"))
            gameObject.SetActive(false);

        // Destroy at impact || destroy after seconds
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Wallll");
            if (!isArrow)
            {
                if (!isGhost && !isLaser)
                    gameObject.SetActive(false);
                else if (isGhost)
                    gameObject.SetActive(false);
            }

            // Arrow behaviour (Stay stuck on wall)
            else if (isArrow)
            {
                speed = 0;
                gameObject.SetActive(false);
            }

            if (isLaser)
            {
                // Laser behaviour (Reflect on walls)
                transform.Rotate(0, 0, -45);
                transform.right = -transform.right;
            }

            //gameObject.SetActive(false);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            //substract enemy life
            Enemy enemyHit = collision.gameObject.GetComponent<Enemy>();
            enemyHit.enemyMembership = membership;
            enemyHit.life -= damage;

            if(destroyOnEnemyCollision)
                gameObject.SetActive(false);
        }
    }
}
