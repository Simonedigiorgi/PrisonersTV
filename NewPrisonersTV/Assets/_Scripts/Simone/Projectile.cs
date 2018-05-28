using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    private Rigidbody2D rb;

	void Start () {

        rb = GetComponent<Rigidbody2D>();
    }
	
	void Update () {

        rb.velocity = transform.right * 20;
    }
}
