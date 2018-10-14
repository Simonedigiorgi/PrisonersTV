using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunParticleBullet : ParticleEmitterRaycastBullet
{
    
    public override void EmitBullet(Transform spawnPoint)
    {
        //ParticleSystem.MainModule psMain = Gun.main;
        //psMain.startColor = bulletColor.Evaluate(colorRange);
        // psMain.gravityModifier = bulletGravity;
        transform.position = spawnPoint.position;
        transform.rotation = Quaternion.LookRotation(spawnPoint.right,spawnPoint.up);
        Debug.Log(transform.position);  
        Gun.Emit(1);  
    }

}
