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
    private Animator anim;
    private bool spawnDone = true;

	// Use this for initialization
	void Start ()
    {
        anim = GetComponent<Animator>();
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

            if (GMController.instance.GetEnemyCount() < GMController.instance.maxEnemy && timer <= 0 && spawnDone)
            {
                if (spawnType == ENEMYTYPE.Random)
                {
                    int i = Random.Range((int)ENEMYTYPE.Bat, (int)ENEMYTYPE.Ninja);
                    if (i == (int)ENEMYTYPE.Bat)
                    {
                       StartCoroutine(SpawnBat());
                    }
                    else if (i == (int)ENEMYTYPE.Ninja)
                    {
                        SpawnNinja();
                    }
                }
                else if (spawnType == ENEMYTYPE.Bat && GMController.instance.GetBatsCount() < GMController.instance.maxBats)
                {
                    StartCoroutine(SpawnBat());
                }
                else if (spawnType == ENEMYTYPE.Ninja && GMController.instance.GetNinjaCount() < GMController.instance.maxNinja)
                {
                    SpawnNinja();
                }
            }
        }
	}
    public void ResetTimer()
    {
        timer = spawnTimer;
    }

    private IEnumerator SpawnBat()
    {
        spawnDone = false;
        GMController.instance.AddBatsCount(); // add bats count
        anim.SetInteger("State",1);
        yield return new WaitForSeconds(0.2f);
        GameObject newEnemy = Instantiate(enemyList.Bat[spawnLevel-1].gameObject,transform.position,Quaternion.identity);
        GMController.instance.allEnemies.Add(newEnemy.GetComponent<_EnemyController>()); // add to enemies list
        anim.SetInteger("State", 2);
        yield return new WaitForSeconds(0.2f);
        anim.SetInteger("State", 0);
        timer = spawnTimer;
        spawnDone = true;
        yield return null;
    }

    private void SpawnNinja()
    {

    }

}
