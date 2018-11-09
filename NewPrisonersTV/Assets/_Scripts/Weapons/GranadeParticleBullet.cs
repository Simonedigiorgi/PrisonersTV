using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class GranadeParticleBullet : ParticleEmitterRaycastBullet
{
    public LayerMask explosionMask;
    protected override void Awake()
    {
        base.Awake();
        childParticle = transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    public override void EmitBullet(Transform spawnPoint)
    {
        ParticleSystem.MainModule psMain = Gun.main;
        //Stat changes
        psMain.startLifetime = weapon.bulletLifeTime;
        psMain.startSpeed = weapon.bulletSpeed;
        psMain.gravityModifier = weapon.bulletGravity;        

        // emission
        transform.position = spawnPoint.position;
        transform.rotation = Quaternion.LookRotation(spawnPoint.right,spawnPoint.up);
        Gun.Emit(1);
    }

    protected override void EmissionHandler()
    {
        if (bullets == null || bullets.Length < Gun.main.maxParticles)
            bullets = new ParticleSystem.Particle[Gun.main.maxParticles];

        int numParticlesAlive = Gun.GetParticles(bullets);

        // cast ray from all the particles that are alive ti register hits 
        for (int i = 0; i < numParticlesAlive; i++)
        {
            bool alreadyExploded = false;
            RaycastHit2D hit = Physics2D.Raycast(bullets[i].position, bullets[i].velocity.normalized, weapon.rayLenght, weapon.obstacleMask);
            Debug.DrawRay(bullets[i].position, bullets[i].velocity.normalized, Color.blue);
            if (hit)
            {
                if (hit.transform.CompareTag("Enemy"))
                {                                   

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(bullets[i].position, childParticle.shape.radius, explosionMask);
                    DamageDealer(colliders); 
                    alreadyExploded = true;
                    bullets[i].remainingLifetime = 0;
                }
                else if (weapon.canBounce)
                {
                    CheckBulletLife(hit, i);
                    // bounce here
                    Vector2 hitNorm = hit.normal;
                    Vector2 newDir = Vector2.Reflect(bullets[i].velocity, hitNorm);
                    bullets[i].velocity = newDir;
                }
                else
                {
                    if (weapon.leaveDecal)
                        SpawnDecal(hit, i);

                    bullets[i].remainingLifetime = 0;      
                }
            }
            if(bullets[i].remainingLifetime <= 0.1 && !alreadyExploded)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(bullets[i].position, childParticle.shape.radius, explosionMask);
                DamageDealer(colliders);
                alreadyExploded = true;
                bullets[i].remainingLifetime = 0;
            }
        }
        // Apply the particle changes to the particle system
        Gun.SetParticles(bullets, numParticlesAlive);
    }

}
