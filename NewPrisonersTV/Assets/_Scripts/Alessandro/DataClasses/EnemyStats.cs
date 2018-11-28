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
    [Tooltip("Movement speed")] public int attackValue;
    [Tooltip("Weakness")] public ResistanceAndWeakness[] weakness;
    [Tooltip("Resistance")] public ResistanceAndWeakness[] resistance;
    //---------------------------------------------------------------------------------------
    [BoxGroup("Jump between NavLinks")] public float jumpSpeed;

    #region BATS
    [Range(0.5f, 3)]
    [BoxGroup("Bat Only")][Tooltip("Time needed for the swoop")] public float swoopMoreSlowly;
    [BoxGroup("Bat Only")] [Tooltip("Initial movement directions")] public STARTDIRECTION myStartDirection;
    [BoxGroup("Bat Only")][Tooltip("Amplitude of sinusoidal movement")] public int sinusoidalMovement;
    #endregion

    #region NINJA
    [BoxGroup("Ninja Only")][Tooltip("Time in second needed between one shuriken and other shuriken")] public float ShurikenCooldown;
    [BoxGroup("Ninja Only")] public LayerMask shurikenHitMask;
    [BoxGroup("Ninja Only")] public int shurikenNumberofHits;
    [BoxGroup("Ninja Only")] public bool canBounce;
    [BoxGroup("Ninja Only")] public float shurikenLifeTime;
    [BoxGroup("Ninja Only")] public float shurikenSpeed;
    [BoxGroup("Ninja Only")] public float shurikenGravity;
    [BoxGroup("Ninja Only")] public float ShurikenRayLenght;
    [BoxGroup("Ninja Only")] public float ninjaJumpLenght;
    [BoxGroup("Ninja Only")] public float ninjaJumpHeight;
    [BoxGroup("Ninja Only")] public float ninjaJumpCooldown;
    [BoxGroup("Ninja Only")] public float groundCheckRadius;
    #endregion

    #region KAMIKAZE
    [BoxGroup("Kamikaze Only")] public float explosionTimer;
    [BoxGroup("Kamikaze Only")] public float runSpeed;
    #endregion
    [BoxGroup("Kamikaze and Spider")] public LayerMask explosionMask;
}
