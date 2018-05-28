using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Sirenix.OdinInspector;

public class Weapon : MonoBehaviour {

    private AudioSource source;                                                                                 // Get the Audiosource component

    private BoxCollider2D coll;                                                                                 // Weapon collider
    private GameObject hand;                                                                                    // Get the Player hand (need to work on this to make Player 2)                                                                                        
    private Transform spawnPoint;                                                                               // Where the bullet is istantiate

    [BoxGroup("Weapon bullet")] public GameObject bullet;                                                       // Bullet gameobject

    [BoxGroup("Controls")] public float fireRate;                                                               // Rate of fire
    [BoxGroup("Controls")] public bool autoFire;                                                                // Has autofire                                                                    

    [BoxGroup("Sounds")] public AudioClip shootSound;                                                           // Shoot sound
    [BoxGroup("Sounds")] [Range(0.1f, 1f)] public float shootVolume;
    [BoxGroup("Sounds")] public AudioClip grabSound;                                                            // Grab sound
    [BoxGroup("Sounds")] [Range(0.1f, 1f)] public float grabVolume;

    private float lastShot = 0.0f;

    [HideInInspector] public bool isGrabbed;                                                                    // Grab the weapon

    private void Start()
    {
        source = GetComponent<AudioSource>();

        coll = transform.GetChild(1).GetComponent<BoxCollider2D>();
        hand = GameObject.Find("Hand");

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

        // Rotate the weapon when equipped
        if (FindObjectOfType<PlayerController>().facingRight && isGrabbed)
            transform.localEulerAngles = new Vector3(180, 0, 0);
        else if (FindObjectOfType<PlayerController>().facingRight == false && isGrabbed)
            transform.localEulerAngles = new Vector3(0, 0, 0);

        // Single shoot
        if (!autoFire)
            if (Input.GetButtonDown("Fire2") && isGrabbed)
                Shoot();
        // Autofire
        if (autoFire)
            if (Input.GetButton("Fire2") && isGrabbed)
                Shoot();
    }

    // Get the weapon and destroy the previously when get another
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player_1") && !isGrabbed)
        {
            source.PlayOneShot(grabSound, grabVolume);

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

    // Shoot method
    public void Shoot()
    {
        if (Time.time > fireRate + lastShot)
        {
            source.PlayOneShot(shootSound, shootVolume);

            Instantiate(bullet, spawnPoint.transform.position, spawnPoint.transform.rotation);
            lastShot = Time.time;


        }
    }
}
