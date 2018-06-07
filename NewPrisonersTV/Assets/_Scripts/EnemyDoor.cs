using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDoor : MonoBehaviour
{
    [Tooltip("the enemies that the trapdoor can spawn")]
    public GameObject[] enemies;

    [Tooltip("the time that the enemy takes for respawn after death")]
    public sbyte timeToRespawn; 

    GameObject myEnemyInGame;

    Animator myAnimator;

    bool coroutineInExecution = false;
    bool canSpawn = false;


	// Use this for initialization
	void Start ()
    {
        myAnimator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(!coroutineInExecution && canSpawn && myEnemyInGame == null)
        {
            coroutineInExecution = true;
            canSpawn = false;
            StartCoroutine(Spawn());
        }

        if(myEnemyInGame == null)
        {
            canSpawn = true;
        }
	}

    IEnumerator Spawn()
    {
        //set door open animation
        myAnimator.SetInteger("State", 1);
        yield return new WaitForSeconds(timeToRespawn);

        //spawn random enemies
        myEnemyInGame = Instantiate(enemies[Random.Range(0, enemies.Length)], transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);

        //set door closed animation
        myAnimator.SetInteger("State", 2);
        yield return new WaitForSeconds(1f);

        //set door default animation
        myAnimator.SetInteger("State", 0);
        coroutineInExecution = false;
    }
}
