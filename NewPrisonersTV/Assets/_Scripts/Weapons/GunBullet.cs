using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GunBullet : Bullet
{
    [BoxGroup("Controls")] public int myDamage;



    private void Awake()
    {
        damage = myDamage;
        destroyOnEnemyCollision = true;
        dir = transform.right;
        colliderBoundX = gameObject.GetComponent<Collider2D>().bounds.size.x;
    }


    void Update()
    {
        // Direction
        transform.Translate(dir * speed * Time.deltaTime, Space.World);

        // Raycast check for wall Collision
        EnvoiromentRaycastCheck();

    }


    protected override void EnvoiromentRaycastCheck()
    {
        Vector2 rayDirection = dir;
        Debug.DrawRay(transform.position + (-transform.right * colliderBoundX / 2), rayDirection, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(transform.position + (-transform.right * colliderBoundX / 2), rayDirection, 1.0f, obstacleMask);
        if (hit && canBounce)
        {

            Vector2 hitNorm = hit.normal;
            newDir = Vector2.Reflect(dir, hitNorm);
            dir = newDir;

            //take angle of shoot
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            //rotate shuriken to target
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        }
        else if (hit && !canBounce)
        {
            Destroy(gameObject);
        }

    }
}
    

