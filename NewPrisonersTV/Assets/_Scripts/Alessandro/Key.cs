using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character;

public class Key : Weapon3D
{
    public override void Shoot(GameObject spawnPoint)
    {

    }

    protected override void GrabWeapon(_CharacterController player)
    {
        isGrabbed = true;
        rb.isKinematic = true;
        transform.parent = hand.transform;
        transform.position = hand.transform.position;
        coll.enabled = false;
        player.currentWeapon = GetComponent<Key>(); 
    }
}
