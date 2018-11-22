using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Prototype/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    public int life;
    [Range(1, 3)]
    [Tooltip("Enemy start level")] public sbyte enemyLevel = 1;
    [Tooltip("Value off point earned by the player")] public int points;
    [Tooltip("View distance")] public sbyte attackView;
    [Tooltip("view obstacle")] public LayerMask obstacleMask;
    [Tooltip("Movement speed")] public int speed;
    [Tooltip("Movement speed")] public int attackValue;
    [Tooltip("Weakness")] public ResistanceAndWeakness[] weakness;
    [Tooltip("Resistance")] public ResistanceAndWeakness[] resistance;
    //---------------------------------------------------------------------------------------
    #region BATS
    [Range(0.5f, 3)]
    [BoxGroup("Bat Only")][Tooltip("Time needed for the swoop")] public float swoopMoreSlowly;
    [BoxGroup("Bat Only")] [Tooltip("Initial movement directions")] public STARTDIRECTION myStartDirection;
    [BoxGroup("Bat Only")][Tooltip("Amplitude of sinusoidal movement")] public int sinusoidalMovement;
    #endregion

    #region NINJA
    [BoxGroup("Ninja Only")][Tooltip("Time in second needed between one shuriken and other shuriken")] public sbyte ShurikenCooldown;
    #endregion

    #region KAMIKAZE
    [BoxGroup("Kamikaze Only")] public float explosionTimer;
    [BoxGroup("Kamikaze Only")] public float runSpeed;
    [BoxGroup("Kamikaze Only")] public float jumpSpeed;
    #endregion


}
