using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class PerforationBullet : MonoBehaviour
{
    public BULLETTYPE type;
    public int poolSize;
    public bool useTripleRaycast;
    [Range(0,1)]
    public float rayPositionForw;
    [Range(0, 1)]
    public float rayPositionSide;
    public BulletInfo[] perfPool;

    [HideInInspector] public DAMAGETYPE[] damageType;
    [HideInInspector] public int damage;
    [HideInInspector] public float bulletLifeTime;
    [HideInInspector] public bool canBounce;
    [HideInInspector] public float gravityMulti;
    [HideInInspector] public bool magazineReady = false;
    [HideInInspector] int playerNumber;
    [HideInInspector] int lastWeaponNumberOfHits;

    float colliderBoundX;
    Weapon3D playerWeapon = null;
    Vector3[] coord;
    int rayNumber = 3;

    private void Start()
    {      
        playerNumber = transform.parent.GetSiblingIndex();
        PoolFilling();
        coord = new Vector3[rayNumber];
    }

    private void Update()
    {
        if (GMController.instance.gameStart && magazineReady)
        {
            if (GMController.instance.playerInfo[playerNumber].playerController.currentWeapon != null && 
                GMController.instance.playerInfo[playerNumber].playerController.currentWeapon.perfBullet == type &&
                playerWeapon == null)
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
                        perfPool[i].velocity = (perfPool[i].dir * playerWeapon.bulletSpeed);
                        //perfPool[i].velocity += Physics2D.gravity *  gravityMulti * Time.deltaTime;
                        perfPool[i].bullet.transform.Translate(perfPool[i].velocity * Time.deltaTime, Space.World);
                        //perfPool[i].bullet.transform.position += new Vector3 (perfPool[i].velocity.x, perfPool[i].velocity.y,0) * Time.deltaTime;

                        // timer CD
                        perfPool[i].lifeTime -= Time.deltaTime; 

                        // Raycast check for wall Collision 
                        EnvoiromentRaycastCheck(i);

                        if (perfPool[i].numberOfHits <= 0 || perfPool[i].lifeTime <= 0)
                            Collector(i);
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
                perfPool[i].enemyHit = new _EnemyController[perfPool[i].numberOfHits];
                perfPool[i].isActive = true;
                break;
            }
        }
    }
    private void Collector(int i)
    {
        perfPool[i].isActive = false;
        perfPool[i].bullet.transform.position = transform.position;
        perfPool[i].lifeTime = 0;
        perfPool[i].numberOfHits = 0;
        perfPool[i].enemyHit = null;

    }
    private void CopyStats()
    {
        if (playerWeapon.numberOfHits != lastWeaponNumberOfHits)
            lastWeaponNumberOfHits = playerWeapon.numberOfHits;
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
        if (playerWeapon.bulletGravity != gravityMulti)
            gravityMulti = playerWeapon.bulletGravity;
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
        // direction
        perfPool[i].dir = perfPool[i].bullet.transform.right;
        Vector2 rayDirection = perfPool[i].dir;


        if (useTripleRaycast)
        {
            bool hasHit = false;
            RayGridControl(perfPool[i].bullet);
            
            for (int y = 0; y < coord.Length; y++)
            {
                Debug.DrawRay(perfPool[i].bullet.transform.position + (-perfPool[i].bullet.transform.right * rayPositionForw) + coord[y], rayDirection, Color.red);
                RaycastHit2D hit = Physics2D.Raycast(perfPool[i].bullet.transform.position + (-perfPool[i].bullet.transform.right * rayPositionForw) + coord[y], rayDirection, 1.0f, playerWeapon.obstacleMask);
                if (hit)
                {
                    hasHit = true;
                    RaycastHitApply(hit.transform, hit.normal, i);
                    break;
                }
            }

            if (!hasHit)
            {
                if (perfPool[i].enemyHit != null) 
                    perfPool[i].enemyHit = new _EnemyController[lastWeaponNumberOfHits];
            }
        }
        else
        {
            Debug.DrawRay(perfPool[i].bullet.transform.position + (-perfPool[i].bullet.transform.right * rayPositionForw), rayDirection, Color.red);

            RaycastHit2D hit = Physics2D.Raycast(perfPool[i].bullet.transform.position + (-perfPool[i].bullet.transform.right * rayPositionForw), rayDirection, 1.0f, playerWeapon.obstacleMask);
            if (hit)
            {               
                RaycastHitApply(hit.transform, hit.normal, i); 
            }
            else
            {
                if (perfPool[i].enemyHit != null)
                    perfPool[i].enemyHit = new _EnemyController[lastWeaponNumberOfHits];
            }
        }

    }
    private void RaycastHitApply(Transform hit, Vector3 normal,int i)
    {
        if (hit.CompareTag("Enemy"))
        {
            _EnemyController enemy = hit.GetComponent<_EnemyController>();

            if (System.Array.IndexOf(perfPool[i].enemyHit, enemy) == -1)
            {
                int index = System.Array.IndexOf(perfPool[i].enemyHit, null);
                if (index >= 0)
                {
                    perfPool[i].enemyHit[index] = enemy;
                    DoDamage(perfPool[i].enemyHit[index], i);
                }
            }
        }
        else if (canBounce)
        {
            Vector2 hitNorm = normal;
            perfPool[i].newDir = Vector2.Reflect(perfPool[i].dir, hitNorm);
            perfPool[i].dir = perfPool[i].newDir;

            //take angle of shoot
            float angle = Mathf.Atan2(perfPool[i].dir.y, perfPool[i].dir.x) * Mathf.Rad2Deg;

            //rotate bullet to target
            perfPool[i].bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            perfPool[i].numberOfHits--;
        }
        else if (!canBounce)
        {
            Collector(i);
        }
    }
    private void RayGridControl(GameObject bullet)
    {
        coord[0] = bullet.transform.up * rayPositionSide ;
        coord[1] = Vector3.zero;
        coord[2] = -bullet.transform.up * rayPositionSide;
    }

}
