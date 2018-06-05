using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Transform[] weaponsSpawn;
    public GameObject[] weapons;

    public GameObject player1;
    public Transform player1Spawn;
    public GameObject player2;
    public Transform player2Spawn;

    public bool isPlayer1alive;
    public bool isPlayer2alive;

    public int P1Score = 0;
    public int P2Score = 0;

    void Start () {

        // Set player alive to false (need for the GameManager to respawn the players)
        if (player1)
            isPlayer1alive = false;
        else if (player2)
            isPlayer2alive = false;
    }
	
	void Update () {

        Player1Active();
        Player2Active();
    }

    public void Player1Active()
    {
        if (player1.activeInHierarchy == false && !isPlayer1alive)
        {
            if (Input.GetButtonDown("Player1_Start"))
            {
                player1.transform.position = player1Spawn.position;
                player1.SetActive(true);
                isPlayer1alive = true;
            }
        }
        else
            isPlayer1alive = false;
    }

    public void Player2Active()
    {
        if (player2.activeInHierarchy == false && !isPlayer2alive)
        {
            if (Input.GetButtonDown("Player2_Start"))
            {
                player2.transform.position = player2Spawn.position;
                player2.SetActive(true);
                isPlayer2alive = true;
            }
        }
        else
            isPlayer2alive = false;
    }

}
