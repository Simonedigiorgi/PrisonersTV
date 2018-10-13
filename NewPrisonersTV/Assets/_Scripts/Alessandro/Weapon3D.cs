﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Sirenix.OdinInspector;
using Character;

public class Weapon3D : MonoBehaviour
{
    public GameObject hand;                                                                                    // Get the Player hand                                                                                        
    public Animator anim;
    public BulletList bulletList;

    protected AudioSource source;                                                                                 // Get the Audiosource component
    protected BoxCollider2D coll;
    protected BoxCollider2D collTrigger;// Weapon collider
    protected Rigidbody2D rb;

    [BoxGroup("Weapon bullet")] public BULLETTYPE bulletType; 
    public ParticleEmitterRaycastBullet bullet;                                     // bullet;                                                       // Bullet gameobject
    public Transform bulletSpawnPoint;

    [BoxGroup("Controls")] public float fireRate;                                                               // Rate of fire
    [BoxGroup("Controls")] public int bullets;                                                                  // How many bullets remaining  

    [BoxGroup("King of weapon")] public bool autoFire;                                                          // Has autofire     

    [BoxGroup("Sounds")] public AudioClip shootSound;                                                           // Shoot sound
    [BoxGroup("Sounds")] [Range(0.1f, 1f)] public float shootVolume;                                            // Shoot volume

    [BoxGroup("Sounds")] public AudioClip grabSound;                                                            // Grab sound
    [BoxGroup("Sounds")] [Range(0.1f, 1f)] public float grabVolume;                                             // Grab volume

    [BoxGroup("Sounds")] public AudioClip emptySound;                                                           // Grab sound
    [BoxGroup("Sounds")] [Range(0.1f, 1f)] public float emptyVolume;                                            // Grab volume

    [HideInInspector] public bool isGrabbed;                                                                    // The weapon is grabbed
    private float lastShot = 0.0f;                                                                              // Need to be always at 0;

    [HideInInspector] public int weaponMembership;                                                                                       // Grabbed on player1 or player2
    [HideInInspector] public WeaponSpawn currentSpawn; 

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        source = GetComponent<AudioSource>();
        coll = transform.GetChild(0).GetComponent<BoxCollider2D>();
        collTrigger = GetComponent<BoxCollider2D>();
        // create and place bullet type
        //bullet = Instantiate(bulletList.bulletTypes[(int)bulletType].transform.gameObject, transform.position, Quaternion.identity).GetComponent<ParticleEmitterRaycastBullet>();
        //bullet.transform.parent = transform;
        //bullet.transform.rotation = Quaternion.LookRotation(transform.right, transform.up);
       
    }

    // Shoot method
    public virtual void Shoot(GameObject spawnPoint)
    {
        if (bullets != 0)
        {
            if (Time.time > fireRate + lastShot)
            {
                // Instantiate the bullet               
                //Instantiate(bullet, new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y + .15f, spawnPoint.transform.position.z), spawnPoint.transform.rotation);
                bullet.EmitBullet();
                bullets--;

                // Assign the bullet membership
                //bullet.membership = weaponMembership;

                // Delay
                lastShot = Time.time;

                // Shoot sound
                source.PlayOneShot(shootSound, shootVolume);

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

    public virtual void GrabAndDestroy(_CharacterController player)
    {
        // Grab sound
        source.PlayOneShot(grabSound, grabVolume);
        coll.enabled = false;
        collTrigger.enabled = false;
        // Destroy the first && get the second
        player.DestroyCurrentWeapon();

        if (hand.transform.childCount <= 1)
        {
            GrabWeapon(player);
        }
    }

    protected virtual void GrabWeapon(_CharacterController player)
    {
        isGrabbed = true;
        rb.isKinematic = true;
        transform.parent = hand.transform;
        transform.position = hand.transform.position;
        coll.enabled = false;

        player.currentWeapon = GetComponent<Weapon3D>();
        bullet.membership = weaponMembership;

        currentSpawn.SetCurrentWeaponToNull();
        currentSpawn.ResetTimer();
    }
}
