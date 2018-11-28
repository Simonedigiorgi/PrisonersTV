using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using AI;

public class ParticleEmitterRaycastBullet : MonoBehaviour
{
    [BoxGroup("Components")] public ParticleSystem Gun;
    [BoxGroup("Components")] public Weapon3D weapon;

    // copy variables from the gun to continue to apply effects to live particles after the gun is dropped
    protected DAMAGETYPE[] damageType;
    protected int damage;
    protected bool canBounce;
    protected bool leaveDecal;
    protected DecalHandler currentDecal;
    [HideInInspector] public int membership;

    protected ParticleSystem childParticle; // used to store child particle if needed
    protected ParticleSystem.Particle[] bullets;
    protected float percentageOfHits;
  
    protected virtual void Awake()
    {
        percentageOfHits = 1f / weapon.numberOfHits * 100;
        damage = weapon.damage;
        damageType = weapon.damageType;
        canBounce = weapon.canBounce;
        leaveDecal = weapon.leaveDecal;
    }

    public virtual void EmitBullet(Transform spawnPoint)
    {
        ParticleSystem.MainModule psMain = Gun.main;
        //Stat changes
        ApplyStats(psMain);

        // emission
        transform.position = spawnPoint.position;
        transform.rotation = Quaternion.LookRotation(spawnPoint.right, spawnPoint.up);
        Gun.Emit(1);
    }
    public virtual void EmitBullet(Transform spawnPoint, float offset)
    {
        ParticleSystem.MainModule psMain = Gun.main;
        //Stat changes
        ApplyStats(psMain);
        // emission
        Vector3 pos = spawnPoint.position;
        pos += transform.up * offset;
        transform.position = pos;
        transform.rotation = Quaternion.LookRotation(spawnPoint.right, spawnPoint.up);
        Gun.Emit(1);
        // emission
        pos = spawnPoint.position;
        pos -= transform.up * offset;
        transform.position = pos;
        transform.rotation = Quaternion.LookRotation(spawnPoint.right, spawnPoint.up);
        Gun.Emit(1);
    }

    protected void ApplyStats(ParticleSystem.MainModule psMain)
    {
        //Stat changes
        psMain.startLifetime = weapon.bulletLifeTime;
        psMain.startSpeed = weapon.bulletSpeed;
        psMain.gravityModifier = weapon.bulletGravity;
        currentDecal = weapon.currentDecal;
    }

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
        Debug.Log(tempDmg + "  " + enemyHit.currentLife);
    }

    protected void CheckBulletLife(RaycastHit2D hit, int i)
    {
        float bulletLife = bullets[i].remainingLifetime -= bullets[i].remainingLifetime * percentageOfHits / 100;
        if (bulletLife <= 0 && leaveDecal) // check if it's the last hit and leave a decal
        {
            currentDecal.PlaceDecal(hit, bullets[i].velocity);
        }
        bullets[i].remainingLifetime -= bullets[i].remainingLifetime * percentageOfHits / 100;
    }

    protected virtual void EmissionHandler()
    {
        if (bullets == null || bullets.Length < Gun.main.maxParticles)
            bullets = new ParticleSystem.Particle[Gun.main.maxParticles];

        int numParticlesAlive = Gun.GetParticles(bullets);

        // cast ray from all the particles that are alive ti register hits 
        for (int i = 0; i < numParticlesAlive; i++)
        {                  
            RaycastHit2D hit = Physics2D.Raycast(bullets[i].position, bullets[i].velocity.normalized, weapon.rayLenght, weapon.obstacleMask);
            Debug.DrawRay(bullets[i].position, bullets[i].velocity.normalized, Color.blue);
            if (hit)
            {
                if (hit.transform.CompareTag("Enemy"))
                {
                    _EnemyController enemyHit = hit.transform.GetComponent<_EnemyController>();

                    if (leaveDecal)
                        currentDecal.PlaceDecal(hit, bullets[i].velocity, enemyHit);

                    bullets[i].remainingLifetime = 0;

                    DamageDealer(enemyHit);
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
                        currentDecal.PlaceDecal(hit, bullets[i].velocity);

                    bullets[i].remainingLifetime = 0;
                }
            }
        }
        // Apply the particle changes to the particle system
        Gun.SetParticles(bullets, numParticlesAlive);
    }
    
    protected virtual void DamageDealer(_EnemyController enemyHit)
    {
        // check damage type and enemy resistance                 
        int tempDmg = damage;
        CheckDmg(enemyHit, tempDmg);
        enemyHit.enemyMembership = membership;
        enemyHit.currentLife -= tempDmg;
        enemyHit.gotHit = true;
    }
    protected virtual void DamageDealer(Collider2D[] Hit)
    {
        for (int i = 0; i < Hit.Length; i++)
        {
            _EnemyController enemyHit = Hit[i].transform.parent.GetComponent<_EnemyController>();
            // check damage type and enemy resistance                 
            int tempDmg = damage;
            CheckDmg(enemyHit, tempDmg);
            enemyHit.enemyMembership = membership;
            enemyHit.currentLife -= tempDmg;
            enemyHit.gotHit = true;
        }
        Debug.Log("boom");
    }

    protected virtual void Update ()
    {
        EmissionHandler();
        // destroy itself if the weapon is destroyed and there aren't bullets flying
        if (weapon == null && Gun.particleCount == 0)
            Destroy(gameObject);
	}
}
