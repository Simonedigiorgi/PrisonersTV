using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    [Tooltip("The shuriken movement speed")]public sbyte speed;
	
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(transform.right * speed * Time.deltaTime, Space.World);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Player_1") || collision.transform.CompareTag("Player_2") || collision.transform.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
