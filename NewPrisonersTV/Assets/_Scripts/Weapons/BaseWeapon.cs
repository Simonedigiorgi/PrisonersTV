using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Character;

public class BaseWeapon : Weapon3D
{
    [BoxGroup("Base Stuff")] public _CharacterController player;

    protected override void Awake()
    {
        source = GetComponent<AudioSource>();
        weaponMembership = player.playerNumber;
        bullet.membership = weaponMembership;
        bullet.transform.parent = null;
    }
    public override void Shoot(GameObject spawnPoint)
    {
        if (perfBullet == BULLETTYPE.None) // if the perf bullet tag is none force the perforation option to false
            canPerforate = false;
        if (bullet == null && !canPerforate && perfBullet != BULLETTYPE.None) // if there isn't a particle bullet and can't perforate and the perf bullet tag is not NUll then activate perforation
            canPerforate = true;
       
        if (Time.time > fireRate + lastShot)
        {
            // Instantiate the bullet               
            if (!canPerforate)
                bullet.EmitBullet(spawnPoint.transform);
            else
                currentDepot.ShootPerf(spawnPoint.transform);

            // Delay
            lastShot = Time.time;

            // Shoot sound
            source.PlayOneShot(shootSound, shootVolume);

            if (anim != null)
                anim.SetTrigger("Shoot");          
        }      
    }
}
