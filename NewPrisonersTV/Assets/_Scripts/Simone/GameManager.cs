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

    void Start () {
        //StartCoroutine(RandomWeapons());
	}
	
	void Update () {

        if(GameObject.FindGameObjectWithTag("Player_1") == null && !isPlayer1alive)
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

        if (GameObject.FindGameObjectWithTag("Player_2") == null && !isPlayer1alive)
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

    /*public IEnumerator RandomWeapons()
    {
        Random.Range(0, weaponsSpawn.Length);
        yield return new WaitForSeconds(1);
        StartCoroutine(RandomWeapons());
    }*/
}
