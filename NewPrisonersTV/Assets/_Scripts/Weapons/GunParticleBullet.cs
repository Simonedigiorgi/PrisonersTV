using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunParticleBullet : ParticleEmitterRaycastBullet
{
    
    public override void EmitBullet()
    {
        //ParticleSystem.MainModule psMain = Gun.main;
        //psMain.startColor = bulletColor.Evaluate(colorRange);
        // psMain.gravityModifier = bulletGravity;
        Gun.Emit(1); 
    }

}
