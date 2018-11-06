using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class GunParticleBullet : ParticleEmitterRaycastBullet
{   
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
}
