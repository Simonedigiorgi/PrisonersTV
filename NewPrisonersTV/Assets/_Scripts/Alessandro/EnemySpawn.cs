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
    // enemy patrol points
    public Transform[] DogPoints;
    public Transform[] SpiderPoints;
    public Transform[] KamikazePoints;
    public Transform[] NinjaPoints;  

    private float timer;
    private Animator anim;
    private bool spawnDone = true;
    [HideInInspector]public EnemySpawn thisSpawn;

	void Start ()
    {
        anim = GetComponent<Animator>();
        timer = spawnTimer;
        thisSpawn = GetComponent<EnemySpawn>();
	}

    public void ResetTimer()
    {
        timer = spawnTimer;
    }
    public float GetTimer()
    {
        return timer;
    }
    public void TimerCountDown()
    {
        timer -= Time.deltaTime;
    }
    public void SpawnEnemyCheck()
    {
        if (GMController.instance.GetEnemyCount() < GMController.instance.maxEnemy && timer <= 0 && spawnDone)
        {
            if (spawnType == ENEMYTYPE.Random)
            {
                int i = Random.Range((int)ENEMYTYPE.Bat, (int)ENEMYTYPE.Sentinel); // min = first enemy in enum, max = last enemy in enum
                if (i == (int)ENEMYTYPE.Bat && GMController.instance.GetBatsCount() < GMController.instance.maxBats)
                {
                    StartCoroutine(SpawnBat());
                }
                else if (i == (int)ENEMYTYPE.Ninja && GMController.instance.GetNinjaCount() < GMController.instance.maxNinja)
                {
                    StartCoroutine(SpawnNinja());
                }
                else if (i == (int)ENEMYTYPE.Kamikaze && GMController.instance.GetKamikazeCount() < GMController.instance.maxKamikaze)
                {
                    StartCoroutine(SpawnKamikaze());
                }
                else if (i == (int)ENEMYTYPE.Spider && GMController.instance.GetSpidersCount() < GMController.instance.maxSpiders)
                {
                    StartCoroutine(SpawnSpiders());
                }
                else if (i == (int)ENEMYTYPE.Dog && GMController.instance.GetDogsCount() < GMController.instance.maxDogs)
                {
                    StartCoroutine(SpawnDog());
                }
                else if (i == (int)ENEMYTYPE.Sentinel && GMController.instance.GetSentinelCount() < GMController.instance.maxSentinel)
                {
                    StartCoroutine(SpawnSentinel());
                }
            }
            else if (spawnType == ENEMYTYPE.Bat && GMController.instance.GetBatsCount() < GMController.instance.maxBats)
            {
                StartCoroutine(SpawnBat());
            }
            else if (spawnType == ENEMYTYPE.Ninja && GMController.instance.GetNinjaCount() < GMController.instance.maxNinja)
            {
                StartCoroutine(SpawnNinja());
            }
            else if (spawnType == ENEMYTYPE.Kamikaze && GMController.instance.GetKamikazeCount() < GMController.instance.maxKamikaze)
            {
                StartCoroutine(SpawnKamikaze());
            }
            else if (spawnType == ENEMYTYPE.Spider && GMController.instance.GetSpidersCount() < GMController.instance.maxSpiders)
            {
                StartCoroutine(SpawnSpiders());
            }
            else if (spawnType == ENEMYTYPE.Dog && GMController.instance.GetDogsCount() < GMController.instance.maxDogs)
            {
                StartCoroutine(SpawnDog());
            }
            else if (spawnType == ENEMYTYPE.Sentinel && GMController.instance.GetSentinelCount() < GMController.instance.maxSentinel)
            {
                StartCoroutine(SpawnSentinel());
            }
        }
    }

    private IEnumerator SpawnBat()
    {
        spawnDone = false;
        GMController.instance.AddBatsCount(); // add bats count
        anim.SetInteger("State",1);

        yield return new WaitForSeconds(0.2f);
        GameObject newEnemy = Instantiate(enemyList.Bat[spawnLevel-1].gameObject,transform.position,Quaternion.identity);
        _EnemyController controller = newEnemy.GetComponent<_EnemyController>();
        controller.hisEnemySpawn = thisSpawn; // add reference to this spawn
        GMController.instance.allEnemies.Add(controller); // add to enemies list
        anim.SetInteger("State", 2);

        yield return new WaitForSeconds(0.2f);
        anim.SetInteger("State", 0);
        timer = spawnTimer;
        spawnDone = true;
        yield return null;
    }
    private IEnumerator SpawnKamikaze()
    {
        spawnDone = false;
        GMController.instance.AddKamikazeCount(); // add kamikaze count
        anim.SetInteger("State", 1);

        yield return new WaitForSeconds(0.2f);
        GameObject newEnemy = Instantiate(enemyList.Kamikaze[spawnLevel - 1].gameObject, transform.position, Quaternion.identity);
        _EnemyController controller = newEnemy.GetComponent<_EnemyController>();
        controller.hisEnemySpawn = thisSpawn; // add reference to this spawn
        GMController.instance.allEnemies.Add(controller); // add to enemies list
        anim.SetInteger("State", 2);

        yield return new WaitForSeconds(0.2f);
        anim.SetInteger("State", 0);
        timer = spawnTimer;
        spawnDone = true;
        yield return null;
    }
    private IEnumerator SpawnNinja()
    {
        spawnDone = false;
        GMController.instance.AddNinjaCount(); // add ninja count
        anim.SetInteger("State", 1);

        yield return new WaitForSeconds(0.2f);
        GameObject newEnemy = Instantiate(enemyList.Ninja[spawnLevel - 1].gameObject, transform.position, Quaternion.identity);
        _EnemyController controller = newEnemy.GetComponent<_EnemyController>();
        controller.hisEnemySpawn = thisSpawn; // add reference to this spawn
        GMController.instance.allEnemies.Add(controller); // add to enemies list
        anim.SetInteger("State", 2);

        yield return new WaitForSeconds(0.2f);
        anim.SetInteger("State", 0);
        timer = spawnTimer;
        spawnDone = true;
        yield return null;
    }
    private IEnumerator SpawnSpiders()
    {
        spawnDone = false;
        GMController.instance.AddSpidersCount(); // add Spider count 
        anim.SetInteger("State", 1);

        yield return new WaitForSeconds(0.2f);
        GameObject newEnemy = Instantiate(enemyList.Spider[spawnLevel - 1].gameObject, transform.position, Quaternion.Euler(180,0,0));
        _EnemyController controller = newEnemy.GetComponent<_EnemyController>();
        controller.hisEnemySpawn = thisSpawn; // add reference to this spawn
        GMController.instance.allEnemies.Add(controller); // add to enemies list
        anim.SetInteger("State", 2);

        yield return new WaitForSeconds(0.2f);
        anim.SetInteger("State", 0);
        timer = spawnTimer;
        spawnDone = true;
        yield return null;
    }
    private IEnumerator SpawnDog()
    {
        spawnDone = false;
        GMController.instance.AddDogsCount(); // add Dogs count 
        anim.SetInteger("State", 1);

        yield return new WaitForSeconds(0.2f);
        GameObject newEnemy = Instantiate(enemyList.Dog[spawnLevel - 1].gameObject, transform.position, Quaternion.identity);
        _EnemyController controller = newEnemy.GetComponent<_EnemyController>();
        controller.hisEnemySpawn = thisSpawn; // add reference to this spawn
        GMController.instance.allEnemies.Add(controller); // add to enemies list
        anim.SetInteger("State", 2);

        yield return new WaitForSeconds(0.2f);
        anim.SetInteger("State", 0);
        timer = spawnTimer;
        spawnDone = true;
        yield return null;
    }
    private IEnumerator SpawnSentinel()
    {
        spawnDone = false;
        GMController.instance.AddSentinelCount(); // add sentinel count 
        anim.SetInteger("State", 1);

        yield return new WaitForSeconds(0.2f);
        GameObject newEnemy = Instantiate(enemyList.Sentinel[spawnLevel - 1].gameObject, transform.position, Quaternion.identity);
        _EnemyController controller = newEnemy.GetComponent<_EnemyController>();
        controller.hisEnemySpawn = thisSpawn; // add reference to this spawn
        GMController.instance.allEnemies.Add(controller); // add to enemies list
        anim.SetInteger("State", 2);

        yield return new WaitForSeconds(0.2f);
        anim.SetInteger("State", 0);
        timer = spawnTimer;
        spawnDone = true;
        yield return null;
    } 
}
