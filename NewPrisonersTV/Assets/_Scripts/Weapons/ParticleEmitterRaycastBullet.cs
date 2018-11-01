using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using AI;

public abstract class ParticleEmitterRaycastBullet : MonoBehaviour
{
    [BoxGroup("Controls")] public ParticleSystem Gun;
    [BoxGroup("Controls")] public DECALTYPE decalType;
    [BoxGroup("Controls")] public int maxDecals;
    [BoxGroup("Controls")] public DAMAGETYPE[] damageType;
    [BoxGroup("Controls")] public int damage;
    [BoxGroup("Controls")] public float rayLenght = 1;
    [BoxGroup("Controls")] public float bulletGravity;
    [BoxGroup("Controls")] public float bulletSpeed;
    [BoxGroup("Controls")] public int numberOfHits;
    [BoxGroup("Controls")] public LayerMask obstacleMask;
    [BoxGroup("Behaviour")] public bool leaveDecal;
    [BoxGroup("Behaviour")] public bool canPerforate;
    [BoxGroup("Behaviour")] public bool canBounce;

    [HideInInspector] protected ParticleSystem.Particle[] bullets;
    [HideInInspector] public GameObject[] decalPool;
    [HideInInspector] public Vector3 decalScale;
    [HideInInspector] public int membership;
    [HideInInspector] protected int currentHits;
    [HideInInspector] protected int decalUsed = 0;

    protected void Awake()
    {
        if (leaveDecal)
        {
            // Find corresponding decal in the list then instantiate it in the list
            GameObject decal = null;
            for (int i = 0; i < GMController.instance.decalList.depot.Length; i++)
            {
                if(GMController.instance.decalList.depot[i].decalType == decalType)
                {
                    decal = GMController.instance.decalList.depot[i].decal;
                    break;
                }
            }
            decalPool = new GameObject[maxDecals];
            for (int i = 0; i < decalPool.Length; i++)
            {               
                decalPool[i] = Instantiate(decal, GMController.instance.decalDepot);
            }
            decalScale = decal.transform.localScale;
        }
    }

    public abstract void EmitBullet(Transform spawnPoint);
    protected void CheckDmg(_EnemyController enemyHit, int tempDmg)
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
        Debug.Log(tempDmg);
    }

    protected void CheckBulletLife(RaycastHit2D hit, int i, _EnemyController enemyHit)
    {
        float bulletLife = bullets[i].remainingLifetime -= bullets[i].remainingLifetime / currentHits;
        if (bulletLife <= 0 && leaveDecal) // check if it's the last hit and leave a decal
        {
            SpawnDecal(hit, i, enemyHit);
        }
        bullets[i].remainingLifetime -= bullets[i].remainingLifetime -= bullets[i].remainingLifetime / currentHits;
        currentHits--;
    }
    protected void CheckBulletLife(RaycastHit2D hit, int i)
    {
        float bulletLife = bullets[i].remainingLifetime -= bullets[i].remainingLifetime / currentHits;
        if (bulletLife <= 0 && leaveDecal) // check if it's the last hit and leave a decal
        {
            SpawnDecal(hit, i);
        }
        bullets[i].remainingLifetime -= bullets[i].remainingLifetime -= bullets[i].remainingLifetime / currentHits;
        currentHits--;
    }

    protected void SpawnDecal(RaycastHit2D hit, int i, _EnemyController enemyHit)
    {
        if (decalUsed < decalPool.Length)
        {
            decalPool[decalUsed].transform.position = hit.point;
            decalPool[decalUsed].transform.rotation = Quaternion.LookRotation(bullets[i].velocity.normalized); 
            decalPool[decalUsed].transform.parent = hit.transform;
            decalPool[decalUsed].transform.localScale = decalScale;
            
            enemyHit.DecalsOn.Add(decalPool[decalUsed].transform);
            enemyHit.hasDecalsOn = true;

            decalUsed++;
        }
        else
            decalUsed = 0;
    }
    protected void SpawnDecal(RaycastHit2D hit, int i)
    {
        if (decalUsed < decalPool.Length)
        {
            decalPool[decalUsed].transform.position = hit.point;
            decalPool[decalUsed].transform.rotation = Quaternion.LookRotation(bullets[i].velocity.normalized);
            decalPool[decalUsed].transform.parent = hit.transform;
            decalPool[decalUsed].transform.localScale = decalScale;
            decalUsed++;
        }
        else
            decalUsed = 0;
    }

    protected void EmissionHandler()
    {
        if (bullets == null || bullets.Length < Gun.main.maxParticles)
            bullets = new ParticleSystem.Particle[Gun.main.maxParticles];

        int numParticlesAlive = Gun.GetParticles(bullets);
        // cast ray from all the particles that are alive ti register hits 
        for (int i = 0; i < numParticlesAlive; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(bullets[i].position, bullets[i].velocity.normalized, rayLenght, obstacleMask);
            Debug.DrawRay(bullets[i].position, bullets[i].velocity.normalized, Color.blue);
            if (hit)
            {
                if (hit.transform.CompareTag("Enemy"))
                {
                    _EnemyController enemyHit = hit.transform.GetComponent<_EnemyController>();
                    enemyHit.enemyMembership = membership;
                    if (canPerforate)
                    {
                        CheckBulletLife(hit, i, enemyHit);

                        // perforation here
                    }
                    else
                    {
                        if (leaveDecal)
                            SpawnDecal(hit, i, enemyHit);

                        bullets[i].startLifetime = 0;
                    }
                    // check damage type and enemy resistance                 
                    int tempDmg = damage;
                    CheckDmg(enemyHit, tempDmg);

                    enemyHit.currentLife -= tempDmg;
                    enemyHit.gotHit = true;
                }
                else if (canBounce)
                {
                    CheckBulletLife(hit, i);            

                    // bounce here
                    Vector2 hitNorm = hit.normal;
                    Vector2 newDir = Vector2.Reflect(bullets[i].velocity, hitNorm);
                    bullets[i].velocity = newDir;
                }
                else
                {
                    if (leaveDecal)
                        SpawnDecal(hit, i);

                    bullets[i].startLifetime = 0;
                }
            }
        }
        // Apply the particle changes to the particle system
        Gun.SetParticles(bullets, numParticlesAlive);
    }
    

    protected virtual void Update ()
    {
        EmissionHandler();
	}
}
