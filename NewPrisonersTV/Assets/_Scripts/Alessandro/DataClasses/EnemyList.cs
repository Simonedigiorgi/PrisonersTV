using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

[CreateAssetMenu(menuName = "Prototype/EnemyList")]
public class EnemyList : ScriptableObject
{
    public _EnemyController[] Bat;
    public _EnemyController[] Ninja;
    public _EnemyController[] Kamikaze;
    public _EnemyController[] Spider;
    public _EnemyController[] Dog;
    public _EnemyController[] Sentinel;
}

