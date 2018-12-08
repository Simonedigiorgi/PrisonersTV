using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Prototype/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    public int life;
    [Range(1, 3)]
    [Tooltip("Enemy start level")] public int enemyLevel = 1;
    [Tooltip("Value off point earned by the player")] public int points;
    [Tooltip("View distance")] public float attackView;
    [Tooltip("view obstacle")] public LayerMask obstacleMask;
    [Tooltip("Movement speed")] public int speed;
    [Tooltip("Chasing speed")] public float runSpeed;
    [Tooltip("NavLink Speed")] public float jumpSpeed;
    [Tooltip("Damage Value")] public int attackValue;
    [Tooltip("Weakness")] public ResistanceAndWeakness[] weakness;
    [Tooltip("Resistance")] public ResistanceAndWeakness[] resistance;
    //---------------------------------------------------------------------------------------
    [BoxGroup("Bullets & Attacks")] public LayerMask hitMask;
    [BoxGroup("Bullets & Attacks")] public int bulletNumberOfHits;
    [BoxGroup("Bullets & Attacks")] public bool canBulletBounce;
    [BoxGroup("Bullets & Attacks")] public float bulletCooldown;
    [BoxGroup("Bullets & Attacks")] public float bulletLifeTime;
    [BoxGroup("Bullets & Attacks")] public float bulletSpeed;
    [BoxGroup("Bullets & Attacks")] public float bulletGravity;
    [BoxGroup("Bullets & Attacks")] public float bulletRayLenght;
    //---------------------------------------------------------------------------------------
    #region BATS
    [Range(0.5f, 3)]
    [BoxGroup("Bat Only")][Tooltip("Time needed for the swoop")] public float swoopMoreSlowly;
    [BoxGroup("Bat Only")] [Tooltip("Initial movement directions")] public STARTDIRECTION myStartDirection;
    [BoxGroup("Bat Only")][Tooltip("Amplitude of sinusoidal movement")] public int sinusoidalMovement;
    #endregion
    //---------------------------------------------------------------------------------------
    #region NINJA
    [BoxGroup("Ninja Only")] public float ninjaJumpLenght;
    [BoxGroup("Ninja Only")] public float ninjaJumpHeight;
    [BoxGroup("Ninja Only")] public float ninjaJumpCooldown;
    [BoxGroup("Ninja Only")] public float groundCheckRadius;
    #endregion
    //---------------------------------------------------------------------------------------
    #region KAMIKAZE
    [BoxGroup("Kamikaze Only")] public float explosionTimer;
    #endregion
    //---------------------------------------------------------------------------------------
    #region KAMIKAZE & SPIDER
    [BoxGroup("Kamikaze and Spider")] public float bombsTimer;
    [BoxGroup("Kamikaze and Spider")] public float bombsXseconds;
    #endregion
    //---------------------------------------------------------------------------------------
    #region SPIDER
    [BoxGroup("Spider Only")] public int bombsOnDeath;
    #endregion
    //---------------------------------------------------------------------------------------
    #region DOG
    [BoxGroup("Dog Only")] public float triggerBiteDistance;
    [BoxGroup("Dog Only")] public float biteRadius;
    [BoxGroup("Dog Only")] public float biteCooldown;
    [BoxGroup("Dog Only")] public float disengageTimer;
    #endregion
    //---------------------------------------------------------------------------------------
    #region SENTINEL
    [BoxGroup("Sentinel Only")] public float groundRayDistance;
    [BoxGroup("Sentinel Only")] public int numBarrageShots;
    [BoxGroup("Sentinel Only")] public float barrageTimer;
    #endregion
}
