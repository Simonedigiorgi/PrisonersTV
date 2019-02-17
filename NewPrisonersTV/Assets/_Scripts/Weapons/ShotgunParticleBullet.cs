using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using AI;

public class ShotgunParticleBullet : ParticleEmitterRaycastBullet
{
    [BoxGroup("Controls")] public int bulletNumber;

    public override void EmitBullet(Transform spawnPoint, float offset)
    {
        ParticleSystem.MainModule psMain = Gun.main;
        //Stat changes
        ApplyStats(psMain);

        Vector3 dir;
        for (int i = 0; i < bulletNumber; i++)
        {
            // emission
            transform.position = spawnPoint.position;
            dir = spawnPoint.position + spawnPoint.right;
            // set the lower bullet on a straight line 
            if (GMController.instance.playerInfo[membership].PlayerController.facingRight)
                dir -= transform.up * offset * i; 
            else
                dir += transform.up * offset * i;
            // define the new direction for the current bullet
            transform.rotation = Quaternion.LookRotation(dir - spawnPoint.position, spawnPoint.up);
            Gun.Emit(1);
        }       
    }
}
