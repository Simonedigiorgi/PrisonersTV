using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GunBullet : Bullet
{
    [BoxGroup("Controls")] public int myDamage;

    Vector3 dir;
    Vector3 newDir;

    private void Awake()
    {
        damage = myDamage;
        destroyOnEnemyCollision = true;
        dir = transform.right;
    }

    protected override void onWallHit(Collision2D collision)
    {
        //base.onWallHit(collision);
        Debug.Log("Override");
        ContactPoint2D contact = collision.contacts[0];
        Vector3 hitNorm = contact.normal;
        newDir = Vector3.Reflect(dir, hitNorm);
        newDir.z = 0;
        dir = newDir;

    }

    void Update()
    {
        // Direction
        //if(newDir == Vector3.zero)
            transform.Translate(dir * speed * Time.deltaTime, Space.World);
        //else
        //    transform.Translate(newDir * speed * Time.deltaTime, Space.World);

    }
}
