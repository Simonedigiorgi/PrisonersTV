using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Transform[] weaponsSpawn;
    public GameObject[] weapons;

    public GameObject player1;
    public Transform player1Spawn;

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
                Instantiate(player1, player1Spawn.position, Quaternion.identity);
                isPlayer1alive = true;
            }

        }
    }

    /*public IEnumerator RandomWeapons()
    {
        Random.Range(0, weaponsSpawn.Length);
        yield return new WaitForSeconds(1);
        StartCoroutine(RandomWeapons());
    }*/
}
