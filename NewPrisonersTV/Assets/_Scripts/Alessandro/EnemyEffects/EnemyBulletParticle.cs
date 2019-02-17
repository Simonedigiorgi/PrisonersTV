using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;
using Character;
using Sirenix.OdinInspector;

public class EnemyBulletParticle : MonoBehaviour
{
    [BoxGroup("Components")] public _EnemyController owner;
    [BoxGroup("Components")] public ParticleSystem thisParticle;

    protected LayerMask obstacleMask;

    // copy variables from the gun to continue to apply effects to live particles after the gun is dropped
    protected int damage;
    protected bool canBounce;
    protected float percentageOfHits;

    protected ParticleSystem childParticle; // used to store child particle if needed
    protected ParticleSystem.Particle[] bullets;

    protected virtual void Awake()
    {
        percentageOfHits = 1f / owner.m_EnemyStats.bulletNumberOfHits * 100;
        damage = owner.m_EnemyStats.attackValue;
        canBounce = owner.m_EnemyStats.canBulletBounce;
        transform.parent = null;
    }

    public void EmitBullet(Transform spawnPoint, int i)
    {
        ParticleSystem.MainModule psMain = thisParticle.main;
        //Stat changes
        ApplyStats(psMain);

        // emission
        transform.position = spawnPoint.position;
        Vector3 targetDir = GMController.instance.playerInfo[i].PlayerController.TargetForEnemies.position - transform.position;
        Vector2 dir = Vector3.RotateTowards(spawnPoint.position, targetDir, 360f, 0); 
        transform.rotation = Quaternion.LookRotation(dir);
        thisParticle.Emit(1);
    }
    public void EmitBullet(Transform spawnPoint, Vector3 targetDir)
    {
        ParticleSystem.MainModule psMain = thisParticle.main;
        //Stat changes
        ApplyStats(psMain);

        // emission
        transform.position = spawnPoint.position;       
        Vector2 dir = Vector3.RotateTowards(spawnPoint.position, targetDir, 360f, 0);
        transform.rotation = Quaternion.LookRotation(dir);
        thisParticle.Emit(1);
    }


    protected void ApplyStats(ParticleSystem.MainModule psMain)
    {
        //Stat changes
        psMain.startLifetime = owner.m_EnemyStats.bulletLifeTime;
        psMain.startSpeed = owner.m_EnemyStats.bulletSpeed;
        psMain.gravityModifier = owner.m_EnemyStats.bulletGravity;
    }

    protected void CheckBulletLife(RaycastHit2D hit, int i)
    {
        bullets[i].remainingLifetime -= bullets[i].remainingLifetime * percentageOfHits / 100;
    }

    protected void EmissionHandler()
    {
        if (bullets == null || bullets.Length < thisParticle.main.maxParticles)
            bullets = new ParticleSystem.Particle[thisParticle.main.maxParticles];

        int numParticlesAlive = thisParticle.GetParticles(bullets);

        // cast ray from all the particles that are alive ti register hits 
        for (int i = 0; i < numParticlesAlive; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(bullets[i].position, bullets[i].velocity.normalized, owner.m_EnemyStats.bulletRayLenght, owner.m_EnemyStats.hitMask);
            Debug.DrawRay(bullets[i].position, bullets[i].velocity.normalized, Color.blue);
            if (hit)
            {
                if (hit.transform.CompareTag("Player_1"))
                {
                    _CharacterController playerHit = hit.transform.GetComponent<_CharacterController>();                  

                    bullets[i].remainingLifetime = 0;

                    DamageDealer(playerHit);
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
                    bullets[i].remainingLifetime = 0;
                }
            }
        }
        // Apply the particle changes to the particle system
        thisParticle.SetParticles(bullets, numParticlesAlive);
    }

    protected virtual void DamageDealer(_CharacterController playerHit)
    {
        // check damage type and enemy resistance                 
        playerHit.currentLife -= damage;
        if (playerHit.currentLife <= 0)
            playerHit.currentLife = 0;
        GMController.instance.UI.UpdateLifeUI(playerHit.playerNumber); // update life on UI
        GMController.instance.LowerTensionCheck(GMController.instance.tensionStats.playerHitPoints);// sub tension
    }

    protected virtual void Update()
    {
        EmissionHandler();
        // destroy itself if the weapon is destroyed and there aren't bullets flying
        if (owner == null && thisParticle.particleCount == 0)
            Destroy(gameObject);
    }
}
