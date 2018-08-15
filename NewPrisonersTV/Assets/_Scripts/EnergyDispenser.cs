﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDispenser : MonoBehaviour
{
    [Tooltip("the time (in second) in which it remains closed")]
    public sbyte closedTime;

    [Tooltip("how much money needed to purchase energy")]
    public sbyte energyPrice;

    bool Open = true;

    LifeController life1, life2;                                                                            // Get the Player_1 $$ Player_2 LifeController script component

    SpriteRenderer mySpriteRenderer;

    GameManager gameManager;

    void Start ()
    {
        life1 = GameObject.FindGameObjectWithTag("Player_1").GetComponent<LifeController>();
        life2 = GameObject.FindGameObjectWithTag("Player_2").GetComponent<LifeController>();

        mySpriteRenderer = GetComponent<SpriteRenderer>();

        //Find game manager
        if (GameObject.Find("GameManager") != null)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        else
        {
            Debug.Log("ADD GAME MANAGER TO SCENE!!! (named 'GameManager')");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //if player1 is on dispenser
        if (collision.gameObject.CompareTag("Player_1"))
        {
            //if dispenser is open and input is pressed and player1 is damaged and player1 has money
            if (Open && Input.GetButtonDown("Player1_Button Y") && life1.life < 3 && gameManager.P1Score >= energyPrice)
            {
                //close dispenser
                Open = false;

                //add life
                life1.life++;

                //substract money
                gameManager.P1Score -= energyPrice;
                
                StartCoroutine(ResetDispenser());
            }
        }

        //if player1 is on dispenser
        if (collision.gameObject.CompareTag("Player_2"))
        {
            //if dispenser is open and input is pressed and player2 is damaged and player2 has money
            if (Open && Input.GetButtonDown("Player2_Button Y") && life2.life < 3 && gameManager.P2Score >= energyPrice)
            {
                //close dispenser
                Open = false;

                //add life
                life2.life++;

                //substract money
                gameManager.P2Score -= energyPrice;

                StartCoroutine(ResetDispenser());
            }
        }
    }

    //whait time for open dispenser
    IEnumerator ResetDispenser()
    {
        mySpriteRenderer.color = Color.red;

        yield return new WaitForSeconds(closedTime);
        Open = true;
        mySpriteRenderer.color = Color.white;
    }
}
