using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class EnemySpawn : MonoBehaviour
{
    public ENEMYTYPE spawnType;
    [Range(1,3)]
    public int spawnLevel;
    public float spawnTimer;
    public EnemyList enemyList;

    private float timer;

	// Use this for initialization
	void Start ()
    {
        timer = spawnTimer;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (GMController.instance.gameStart)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }

            if (GMController.instance.GetEnemyCount() < GMController.instance.maxEnemy && timer <= 0)
            {
                if (spawnType == ENEMYTYPE.Random)
                {
                    int i = Random.Range((int)ENEMYTYPE.Bats, (int)ENEMYTYPE.Ninja);
                    if (i == (int)ENEMYTYPE.Bats)
                    {
                        SpawnBat();
                    }
                    else if (i == (int)ENEMYTYPE.Ninja)
                    {
                        SpawnNinja();
                    }
                }
                else if (spawnType == ENEMYTYPE.Bats && GMController.instance.GetBatsCount() < GMController.instance.maxBats)
                {
                    SpawnBat();
                }
                else if (spawnType == ENEMYTYPE.Ninja && GMController.instance.GetNinjaCount() < GMController.instance.maxNinja)
                {
                    SpawnNinja();
                }
            }
        }
	}

    private void SpawnBat()
    {
        Instantiate(enemyList.Bat[spawnLevel-1].gameObject,transform.position,Quaternion.identity);
        GMController.instance.AddBatsCount();
        timer = spawnTimer;
    }

    private void SpawnNinja()
    {

    }

}
