﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character;

public class Shotgun3D : Weapon3D
{
    public float spread;

    public override void Shoot(GameObject spawnPoint)
    {
        if (perfBullet == BULLETTYPE.None) // if the perf bullet tag is none force the perforation option to false
            canPerforate = false;
        if (bullet == null && !canPerforate && perfBullet != BULLETTYPE.None) // if there isn't a particle bullet and can't perforate and the perf bullet tag is not NUll then activate perforation
            canPerforate = true;

        if (bullets != 0)
        {
            if (Time.time > fireRate + lastShot)
            {
                // Instantiate the bullet               
                if (!canPerforate)
                    bullet.EmitBullet(spawnPoint.transform, spread);
                else                
                    currentDepot.ShootPerf(spawnPoint.transform);
                
                bullets--;

                // Delay
                lastShot = Time.time;

                // Shoot sound
                source.PlayOneShot(shootSound, shootVolume);

                if (anim != null)
                    anim.SetTrigger("Shoot");
            }
        }
        else if (bullets == 0)
        {
            // Set autofire to false to avoid the annoying sound loop
            autoFire = false;
            source.PlayOneShot(emptySound, emptyVolume);
        }
    }
}
