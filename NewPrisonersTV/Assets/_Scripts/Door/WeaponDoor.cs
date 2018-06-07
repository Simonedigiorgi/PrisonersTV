using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDoor : MonoBehaviour
{
    [Tooltip("the weapons that the door can spawn")]
    public GameObject[] weapons;

    [Tooltip("the time that the weapon takes for respawn after player catch")]
    public sbyte timeToRespawn;

    GameObject myWeaponInGame;

    Animator myAnimator;

    bool coroutineInExecution = false;
    bool canSpawn = false;


    // Use this for initialization
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!coroutineInExecution && canSpawn && myWeaponInGame == null)
        {
            coroutineInExecution = true;
            canSpawn = false;
            StartCoroutine(Spawn());
        }

        if (myWeaponInGame == null)
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
        myWeaponInGame = Instantiate(weapons[Random.Range(0, weapons.Length)], transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);

        //set door closed animation
        myAnimator.SetInteger("State", 2);
        yield return new WaitForSeconds(1f);

        //set door default animation
        myAnimator.SetInteger("State", 0);
        coroutineInExecution = false;
    }
}

