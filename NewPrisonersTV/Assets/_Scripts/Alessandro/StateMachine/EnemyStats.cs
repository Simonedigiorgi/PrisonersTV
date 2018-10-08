using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Prototype/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    public int life;
    [Range(1, 3)]
    [Tooltip("Enemy start level")] public sbyte enemyLevel = 1;
    [Tooltip("Value off point earned by the player")] public int points;
    [Tooltip("View distance")] public sbyte attackView;
    [Tooltip("Bat view obstacle")] public LayerMask obstacleMask;
    [Tooltip("Movement speed")] public int speed;
    [Tooltip("The speed of the flash when enemy is hitted")] public float flashingSpeed;
    [Tooltip("Initial movement directions")] public STARTDIRECTION myStartDirection;
    //bat stats
    [Range(0.5f, 3)]
    [Tooltip("Time needed for the swoop")] public float swoopMoreSlowly;
    [Tooltip("Amplitude of sinusoidal movement")] public int sinusoidalMovement;
    //ninja stats
    [Tooltip("Time in second needed between one shuriken and other shuriken")] public sbyte ShurikenCooldown;


}
