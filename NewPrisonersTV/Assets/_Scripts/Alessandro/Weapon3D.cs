using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Sirenix.OdinInspector;
using Character;
using UnityEngine.UI;

public class Weapon3D : MonoBehaviour
{
    public Animator anim;
    public Image weaponIcon;

    protected AudioSource source;                                                                              // Get the Audiosource component
    protected BoxCollider2D coll;
    protected BoxCollider2D collTrigger;// Weapon collider
    protected Rigidbody2D rb;

    [BoxGroup("Weapon bullet")] public ParticleEmitterRaycastBullet bullet;                                     // bullet;                                                   

    [BoxGroup("Controls")] public float fireRate;                                                               // Rate of fire
    [BoxGroup("Controls")] public int bullets;                                                                  // How many bullets remaining  
    [BoxGroup("Controls")] public int bulletsIfReward;                                                          // How many bullets if it was a reward

    [BoxGroup("King of weapon")] public bool autoFire;                                                          // Has autofire     

    [BoxGroup("Sounds")] public AudioClip shootSound;                                                           // Shoot sound
    [BoxGroup("Sounds")] [Range(0.1f, 1f)] public float shootVolume;                                            // Shoot volume

    [BoxGroup("Sounds")] public AudioClip grabSound;                                                            // Grab sound
    [BoxGroup("Sounds")] [Range(0.1f, 1f)] public float grabVolume;                                             // Grab volume

    [BoxGroup("Sounds")] public AudioClip emptySound;                                                           // Grab sound
    [BoxGroup("Sounds")] [Range(0.1f, 1f)] public float emptyVolume;                                            // Grab volume

    [HideInInspector]public GameObject hand;                                                                    // the Player hand                                                                                        
    [HideInInspector] public bool isGrabbed;                                                                    // The weapon is grabbed
    private float lastShot = 0.0f;                                                                              // Need to be always at 0;

    [HideInInspector] public int weaponMembership;                                                                                       // Grabbed on player1 or player2
    [HideInInspector] public WeaponSpawn currentSpawn;
    [HideInInspector] public bool isReward = false;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        source = GetComponent<AudioSource>();
        coll = transform.GetChild(0).GetComponent<BoxCollider2D>();
        collTrigger = GetComponent<BoxCollider2D>();      
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
                bullet.EmitBullet(spawnPoint.transform);
                bullets--;

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
        bullet.transform.parent = null;
        if (!isReward)
        {
            currentSpawn.SetCurrentWeaponToNull();
            currentSpawn.ResetTimer();
        }
    }
}
