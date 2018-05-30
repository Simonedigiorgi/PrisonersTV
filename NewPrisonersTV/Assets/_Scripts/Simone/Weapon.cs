using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Sirenix.OdinInspector;

public class Weapon : MonoBehaviour {

    private AudioSource source;                                                                                 // Get the Audiosource component

    private BoxCollider2D coll;                                                                                 // Weapon collider
    private GameObject hand;                                                                                    // Get the Player hand                                                                                        
    private Transform spawnPoint;                                                                               // Where the bullet is istantiate

    [BoxGroup("Weapon bullet")] public GameObject bullet;                                                       // Bullet gameobject

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

    private void Start()
    {
        source = GetComponent<AudioSource>();

        coll = transform.GetChild(1).GetComponent<BoxCollider2D>();

        spawnPoint = transform.GetChild(2);                                                                     // Get the Spawnpoint child
    }

    void Update () {

        // Weapon position = player's hand position
        if (isGrabbed)
        {
            transform.position = hand.transform.position;
            transform.parent = hand.transform;
            coll.enabled = false;
        }
    }

    // Get the weapon and destroy the previously when get another
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player_1") && !isGrabbed)
        {
            //Get the player hand
            hand = GameObject.Find("Hand_Player1");

            DestroyWeapon();
        }

        if (collision.gameObject.CompareTag("Player_2") && !isGrabbed)
        {
            //Get the player hand
            hand = GameObject.Find("Hand_Player2");

            DestroyWeapon();
        }
    }

    // Shoot method
    public void Shoot()
    {
        if(bullets != 0)
        {
            bullets--;

            if (Time.time > fireRate + lastShot)
            {
                // Shoot sound
                source.PlayOneShot(shootSound, shootVolume);

                // Instantiate the bullet
                Instantiate(bullet, spawnPoint.transform.position, spawnPoint.transform.rotation);
                lastShot = Time.time;
            }
        }
        else if (bullets == 0)
        {
            // Set autofire to false to avoid the annoying sound loop
            autoFire = false;
            source.PlayOneShot(emptySound, emptyVolume);
        }

    }

    public void DestroyWeapon()
    {
        // Grab sound
        source.PlayOneShot(grabSound, grabVolume);

        // Destroy the first && get the second
        if (hand.transform.childCount <= 1)
        {
            isGrabbed = true;

            if (hand.transform.childCount > 0)
            {
                GameObject first = hand.transform.GetChild(0).gameObject;
                Destroy(first.gameObject);
            }
        }
    }
}
