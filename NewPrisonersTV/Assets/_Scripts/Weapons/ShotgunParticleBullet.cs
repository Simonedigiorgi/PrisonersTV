using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class ShotgunParticleBullet : ParticleEmitterRaycastBullet
{
    public int bulletNumber;

    public override void EmitBullet(Transform spawnPoint, float offset)
    {
        ParticleSystem.MainModule psMain = Gun.main;
        //Stat changes
        psMain.startLifetime = weapon.bulletLifeTime;
        psMain.startSpeed = weapon.bulletSpeed;
        psMain.gravityModifier = weapon.bulletGravity;
        Vector3 dir;
        for (int i = 0; i < bulletNumber; i++)
        {
            // emission
            transform.position = spawnPoint.position;
            dir = spawnPoint.position + spawnPoint.right;
            dir += transform.up * offset * i; 
            transform.rotation = Quaternion.LookRotation(dir - spawnPoint.position, spawnPoint.up);
            Gun.Emit(1);
        }       
    }
}
