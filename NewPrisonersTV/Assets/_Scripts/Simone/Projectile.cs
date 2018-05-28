using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Projectile : MonoBehaviour {

    private Rigidbody2D rb;

    [BoxGroup("Controls")] public float speed;
    [BoxGroup("Kind of weapon")] public bool isArrow;

	void Start () {

        rb = GetComponent<Rigidbody2D>();
    }
	
	void Update () {

        rb.velocity = transform.right * speed * Time.deltaTime;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") && !isArrow)
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Wall") && isArrow)
        {
            Destroy(gameObject, 5);
        }
    }
}
