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
        psMain.startSpeed = bulletSpeed;
        psMain.gravityModifier = bulletGravity;

        if(canBounce || canPerforate)
            currentHits = numberOfHits+1;

        // emission
        transform.position = spawnPoint.position;
        transform.rotation = Quaternion.LookRotation(spawnPoint.right,spawnPoint.up);
        Gun.Emit(1);  
    }
}
