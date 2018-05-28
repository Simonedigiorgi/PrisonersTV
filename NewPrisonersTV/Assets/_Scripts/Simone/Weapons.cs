﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Weapons : MonoBehaviour {

    private BoxCollider2D coll;
    private GameObject hand;
    private Transform spawnPoint;

    [BoxGroup("Weapon bullet")] public GameObject bullet;

    [BoxGroup("Controls")] public float fireRate;
    [BoxGroup("Controls")] public bool autoFire;

    private float lastShot = 0.0f;

    [HideInInspector] public bool isGrabbed;

    private void Start()
    {
        coll = transform.GetChild(1).GetComponent<BoxCollider2D>();
        hand = GameObject.Find("Hand");

        spawnPoint = transform.GetChild(2);
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

        // Shoot
        if (!autoFire)
            if (Input.GetButtonDown("Fire2") && isGrabbed)
                Shoot();
        else
            if (Input.GetButtonDown("Fire2") && isGrabbed)
                Shoot();
    }

    // Get the weapon and destroy the previously when get another
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player_1") && !isGrabbed)
        {
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

    void Shoot()
    {
        if (Time.time > fireRate + lastShot)
        {
            Instantiate(bullet, spawnPoint.transform.position, spawnPoint.transform.rotation);
            lastShot = Time.time;
        }
    }
}
