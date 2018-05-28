using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Bullet : MonoBehaviour {

    private Rigidbody2D rb;

    [BoxGroup("Controls")] public float speed;
    [BoxGroup("Controls")] public float destroyAfter;

    [BoxGroup("Kind of weapon")] public bool isArrow;

	void Start () {

        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, destroyAfter);
    }
	
	void Update () {

        // Movement
        rb.velocity = transform.right * speed * Time.deltaTime;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") && !isArrow)
        {
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") && isArrow)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            Destroy(gameObject, destroyAfter);
        }

        if (collision.gameObject.CompareTag("Bullet") && isArrow)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            Destroy(gameObject);
        }
    }
}
