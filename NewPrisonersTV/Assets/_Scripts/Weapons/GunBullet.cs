﻿using System.Collections;
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
    }

    void Update()
    {
        // Direction
        transform.Translate(transform.right * speed * Time.deltaTime, Space.World);
    }
}
