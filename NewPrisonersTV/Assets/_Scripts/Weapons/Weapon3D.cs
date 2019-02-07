using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Sirenix.OdinInspector;
using Character;
using UnityEngine.UI;

public class Weapon3D : MonoBehaviour
{
    public ParticleSystem muzzle;
    public Image weaponIcon;
    public MeshRenderer mesh;

    protected AudioSource source;                                                                               // Get the Audiosource component
    protected BoxCollider2D coll;
    protected BoxCollider2D collTrigger;                                                                        // Weapon collider
    protected Rigidbody2D rb;

    [BoxGroup("Weapon bullet")] public ParticleEmitterRaycastBullet bullet;                                     // bullet;
    [BoxGroup("Weapon bullet")] public BULLETTYPE perfBullet;

    [BoxGroup("Controls")] public float fireRate;                                                               // Rate of fire
    [BoxGroup("Controls")] public int bullets;                                                                  // How many bullets remaining  
    [BoxGroup("Controls")] public int bulletsIfReward;                                                          // How many bullets if it was a reward
    [BoxGroup("Controls")] public int numberOfHits;
    [BoxGroup("Controls")] public float rayLenght = 1;
    [BoxGroup("Controls")] public float bulletLifeTime;
    [BoxGroup("Controls")] public float bulletGravity;
    [BoxGroup("Controls")] public float bulletSpeed;
    [BoxGroup("Controls")] public LayerMask obstacleMask;
    [BoxGroup("Controls")] public BULLETTYPE decalType;
    [BoxGroup("Controls")] public int damage;
    [BoxGroup("Controls")] public DAMAGETYPE[] damageType;

    [BoxGroup("Behaviour")] public bool leaveDecal;
    [BoxGroup("Behaviour")] public bool canPerforate;
    [BoxGroup("Behaviour")] public bool canBounce;
    [BoxGroup("Behaviour")] public bool autoFire;                                                          // Has autofire     


    [BoxGroup("Sounds")] public AudioClip shootSound;                                                           // Shoot sound
    [BoxGroup("Sounds")] [Range(0.1f, 1f)] public float shootVolume;                                            // Shoot volume

    [BoxGroup("Sounds")] public AudioClip grabSound;                                                            // Grab sound
    [BoxGroup("Sounds")] [Range(0.1f, 1f)] public float grabVolume;                                             // Grab volume

    [BoxGroup("Sounds")] public AudioClip emptySound;                                                           // Grab sound
    [BoxGroup("Sounds")] [Range(0.1f, 1f)] public float emptyVolume;                                            // Grab volume

    [HideInInspector] public GameObject hand;                                                                    // the Player hand                                                                                        
    [HideInInspector] public bool isGrabbed;                                                                    // The weapon is grabbed
    protected float lastShot = 0.0f;                                                                              // Need to be always at 0;

    [HideInInspector] public int weaponMembership;                                                                                       // Grabbed on player1 or player2
    [HideInInspector] public WeaponSpawn currentSpawn;
    [HideInInspector] public bool isReward = false;

    [HideInInspector] public PerforationBullet currentDepot;
    [HideInInspector] public DecalHandler currentDecal;

    #region PARTICLE SYSTEM
    private Transform muzzleT;                              // reference to the muzzle particle transform
    private ParticleSystem.MainModule psMain;               // reference to the main module of the particle, used to change color
    private ParticleSystem.EmissionModule emission;         // reference to the emission module, used to get the burst info and replicate the emission
    protected int minParticles;                               // max particle in burst
    protected int maxParticles;                               // min particle in burst
    #endregion

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        source = GetComponent<AudioSource>();
        coll = transform.GetChild(0).GetComponent<BoxCollider2D>();
        collTrigger = GetComponent<BoxCollider2D>();
        if (muzzle != null)
            GetParticleInfo();
    }

    protected void GetParticleInfo()                                                 // get particle burst info to replicate the emission when needed
    {
        //Get Particle Burst Info
        muzzleT = muzzle.transform;
        emission = muzzle.emission;
        ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[emission.burstCount];
        emission.GetBursts(bursts);
        minParticles = bursts[0].minCount;
        maxParticles = bursts[0].maxCount;
    }

    // Shoot method
    public virtual void Shoot(GameObject spawnPoint) 
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
                if(!canPerforate)
                    bullet.EmitBullet(spawnPoint.transform);
                else              
                    currentDepot.ShootPerf(spawnPoint.transform);

                bullets--;

                // Delay
                lastShot = Time.time;

                // Shoot sound
                source.PlayOneShot(shootSound, shootVolume);

                if (muzzle != null)
                    muzzle.Emit(Random.Range(minParticles,maxParticles));
            }
        }
        else if (bullets == 0)
        {
            // Set autofire to false to avoid the annoying sound loop
            autoFire = false;
            source.PlayOneShot(emptySound, emptyVolume);
            // destroy this weapon and eneable the base one
            GMController.instance.playerInfo[weaponMembership].playerController.EnableBaseWeapon();
        }
    }

    public virtual void GrabAndDestroy(_CharacterController player)
    {
        // Grab sound
        source.PlayOneShot(grabSound, grabVolume);
        collTrigger.enabled = false;
        // Destroy the first  
        player.DestroyCurrentWeapon();
        // get the second
        if (hand.transform.childCount <= 1)// (need to check condition)       
            GrabWeapon(player);
    }

    protected virtual void GrabWeapon(_CharacterController player)
    {
        isGrabbed = true;
        rb.isKinematic = true;
        rb.simulated = false;
        transform.parent = hand.transform;
        transform.position = hand.transform.position;

        player.currentWeapon = GetComponent<Weapon3D>();
        if (bullet != null)
        {
            bullet.membership = weaponMembership;
            bullet.transform.parent = null;
        }
        if (!isReward)
        {
            currentSpawn.SetCurrentWeaponToNull();
            currentSpawn.ResetTimer();
        }
    }
}
