using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour {

    public Weapon stats;

    [HideInInspector] public bool isGrabbed;
    public new BoxCollider2D collider2D;

	void Start () {
		
	}
	

	void Update () {
        if (isGrabbed)
        {
            transform.position = GameObject.Find("Hand").transform.position;
            transform.parent = GameObject.Find("Hand").transform;
            collider2D.enabled = false;
        }
	}

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isGrabbed = true;
        }
    }
}
