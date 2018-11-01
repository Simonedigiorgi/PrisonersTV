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
    [Tooltip("Bat view obstacle")] public LayerMask obstacleMask;
    [Tooltip("Movement speed")] public int speed;
    [Tooltip("The speed of the flash when enemy is hitted")] public float flashingSpeed;
    [Tooltip("Initial movement directions")] public STARTDIRECTION myStartDirection;
    [Tooltip("Weakness")] public ResistanceAndWeakness[] weakness;
    [Tooltip("Resistance")] public ResistanceAndWeakness[] resistance;
    //bat stats
    [Range(0.5f, 3)]
    [BoxGroup("Bat Only")][Tooltip("Time needed for the swoop")] public float swoopMoreSlowly;
    [BoxGroup("Bat Only")][Tooltip("Amplitude of sinusoidal movement")] public int sinusoidalMovement;
    //ninja stats
    [BoxGroup("Ninja Only")][Tooltip("Time in second needed between one shuriken and other shuriken")] public sbyte ShurikenCooldown;


}
