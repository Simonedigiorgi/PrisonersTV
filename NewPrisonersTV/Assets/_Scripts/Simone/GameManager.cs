using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Transform[] weaponsSpawn;
    public GameObject[] weapons;

	void Start () {
        StartCoroutine(RandomWeapons());
	}
	
	// Update is called once per frame
	void Update () {

	}

    public IEnumerator RandomWeapons()
    {
        Random.Range(0, weaponsSpawn.Length);
        yield return new WaitForSeconds(1);
        StartCoroutine(RandomWeapons());
    }
}
