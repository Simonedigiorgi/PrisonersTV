using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour {

    public Weapon stats;

    private BoxCollider2D coll;
    private GameObject hand;

    [HideInInspector] public bool isGrabbed;

    private void Start()
    {
        coll = transform.GetChild(1).GetComponent<BoxCollider2D>();
        hand = GameObject.Find("Hand");
    }

    void Update () {
        if (isGrabbed)
        {
            transform.position = hand.transform.position;
            transform.parent = hand.transform;
            coll.enabled = false;
        }

        // Rotate the weapon when equipped
        if (FindObjectOfType<PlayerController>().facingRight && isGrabbed)
            transform.localEulerAngles = new Vector3(0, 0, 180);
        else if (FindObjectOfType<PlayerController>().facingRight == false && isGrabbed)
            transform.localEulerAngles = new Vector3(180, 0, 180);
    }

    // Get the weapon and destroy the previously when get another
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isGrabbed)
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
}
