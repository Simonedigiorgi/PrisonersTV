﻿using UnityEngine;
using AI;
[System.Serializable]
public class BulletInfo
{
    public BULLETTYPE bulletType;
    public GameObject bullet;

    public bool isActive;
    public int numberOfHits;
    public float lifeTime;
    public _EnemyController[] enemyHit;

    [HideInInspector] public Vector2 dir;
    [HideInInspector] public Vector2 newDir;
    [HideInInspector] public Vector2 velocity;

    public BulletInfo(BULLETTYPE type, GameObject obj)
    {
        bulletType = type;
        bullet = obj;
        numberOfHits = 0;
        isActive = false;
        dir = bullet.transform.right;
        lifeTime = 0;
    }
    public BulletInfo(BULLETTYPE type, GameObject obj, int hits, bool state, int timer)
    {
        bulletType = type;
        bullet = obj;
        numberOfHits = hits;
        isActive = state;
        dir = bullet.transform.forward;
        lifeTime = timer;
    }
}