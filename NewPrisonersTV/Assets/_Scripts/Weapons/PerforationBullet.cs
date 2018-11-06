using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class PerforationBullet : MonoBehaviour
{
    public BULLETTYPE type;
    public int poolSize;
    public BulletInfo[] perfPool;

    [HideInInspector] public DAMAGETYPE[] damageType;
    [HideInInspector] public int damage;
    [HideInInspector] public float bulletLifeTime;
    [HideInInspector] public bool canBounce;
    [HideInInspector] public bool magazineReady = false;
    [HideInInspector] int playerNumber;

    float colliderBoundX;
    Weapon3D playerWeapon = null;

    private void Start()
    {      
        playerNumber = transform.parent.GetSiblingIndex();
        PoolFilling();
    }

    private void Update()
    {
        if (GMController.instance.gameStart && magazineReady)
        {
            if (GMController.instance.playerInfo[playerNumber].playerController.currentWeapon == null ||
                GMController.instance.playerInfo[playerNumber].playerController.currentWeapon.perfBullet == type)
            {
                playerWeapon = GMController.instance.playerInfo[playerNumber].playerController.currentWeapon;
            }

            if (playerWeapon != null)
            {
                CopyStats();
                for (int i = 0; i < perfPool.Length; i++)
                {
                    if (perfPool[i].isActive)
                    { 
                        // Movement
                        colliderBoundX = perfPool[i].col.bounds.size.x;
                        perfPool[i].bullet.transform.Translate(perfPool[i].dir * playerWeapon.bulletSpeed * Time.deltaTime, Space.World);

                        // timer CD
                        perfPool[i].lifeTime -= Time.deltaTime;

                        // Raycast check for wall Collision 
                        EnvoiromentRaycastCheck(i);

                        if (perfPool[i].numberOfHits <= 0 || perfPool[i].lifeTime <= 0)
                        {
                            Collector(i);
                        }
                    }
                }
            }
        }
    }

    public void DoDamage(_EnemyController enemyHit, int i)
    {
        // check damage type and enemy resistance                 
        int tempDmg = damage;
        CheckDmg(enemyHit, tempDmg);
        perfPool[i].numberOfHits--;
        enemyHit.enemyMembership = playerNumber;
        enemyHit.currentLife -= tempDmg;
        enemyHit.gotHit = true;
    }
    public void ShootPerf(Transform spawnPoint)
    {
        for (int i = 0; i < perfPool.Length; i++)
        {
            if(!perfPool[i].isActive)
            {
                perfPool[i].bullet.transform.position = spawnPoint.position;
                perfPool[i].bullet.transform.rotation = Quaternion.LookRotation(spawnPoint.forward, spawnPoint.up);
                perfPool[i].lifeTime = bulletLifeTime;
                perfPool[i].numberOfHits = playerWeapon.numberOfHits;
                perfPool[i].isActive = true;
                break;
            }
        }
    }
    private void Collector(int i)
    {
        perfPool[i].bullet.transform.position = transform.position;
        perfPool[i].isActive = false;
    }
    private void CopyStats()
    {
        if (playerWeapon.currentDepot != this)
            playerWeapon.currentDepot = this;
        if (playerWeapon.damage != damage)
            damage = playerWeapon.damage;
        if (playerWeapon.damageType != damageType)
            damageType = playerWeapon.damageType;
        if (playerWeapon.canBounce != canBounce)
            canBounce = playerWeapon.canBounce;
        if (playerWeapon.bulletLifeTime != bulletLifeTime)
            bulletLifeTime = playerWeapon.bulletLifeTime;
    }
    private void CheckDmg(_EnemyController enemyHit, int tempDmg)
    {
        for (int y = 0; y < damageType.Length; y++)
        {
            for (int z = 0; z < enemyHit.m_EnemyStats.weakness.Length; z++)
            {
                if (damageType[y] == enemyHit.m_EnemyStats.weakness[z].type)
                {
                    tempDmg += enemyHit.m_EnemyStats.weakness[z].value;
                    break;
                }
            }
            for (int z = 0; z < enemyHit.m_EnemyStats.resistance.Length; z++)
            {
                if (damageType[y] == enemyHit.m_EnemyStats.resistance[z].type)
                {
                    tempDmg -= enemyHit.m_EnemyStats.resistance[z].value;
                    break;
                }
            }
        }
        if (tempDmg < 0)
            tempDmg = 0;
        Debug.Log(tempDmg + "  " + enemyHit.currentLife);
    }
    private void PoolFilling()
    {
        BulletInfo perf = null;
        for (int i = 0; i < GMController.instance.bulletDepot.depot.Length; i++)
        {
            if (GMController.instance.bulletDepot.depot[i].bulletType == type)
            {
                perf = GMController.instance.bulletDepot.depot[i];
                break;
            }
        }
        perfPool = new BulletInfo[poolSize];
        for (int i = 0; i < perfPool.Length; i++)
        {
            GameObject bullet = Instantiate(perf.bullet, transform);
            perfPool[i] = new BulletInfo(type, bullet);
        }
        magazineReady = true;
    }
    private void EnvoiromentRaycastCheck(int i)
    {
        perfPool[i].dir = perfPool[i].bullet.transform.right;
        Vector2 rayDirection = perfPool[i].dir;
        Debug.DrawRay(perfPool[i].bullet.transform.position + (-perfPool[i].bullet.transform.right * colliderBoundX / 2), rayDirection, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(perfPool[i].bullet.transform.position + (-perfPool[i].bullet.transform.right * colliderBoundX / 2), rayDirection, 1.0f, playerWeapon.obstacleMask);
        if (hit)
        {
            if (canBounce && !hit.transform.CompareTag("Enemy"))
            {
                perfPool[i].numberOfHits--;

                Vector2 hitNorm = hit.normal;
                perfPool[i].newDir = Vector2.Reflect(perfPool[i].dir, hitNorm);
                perfPool[i].dir = perfPool[i].newDir;

                //take angle of shoot
                float angle = Mathf.Atan2(perfPool[i].dir.y, perfPool[i].dir.x) * Mathf.Rad2Deg;

                //rotate bullet to target
                perfPool[i].bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }     
            else if(!canBounce && !hit.transform.CompareTag("Enemy"))
            {
                Collector(i);
            }

        }
    }
}
