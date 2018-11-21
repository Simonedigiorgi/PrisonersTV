using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

[CreateAssetMenu(menuName = "Prototype/EnemyList")]
public class EnemyList : ScriptableObject
{
    public List<_EnemyController> Bat;
    public List<_EnemyController> Ninja;
    public List<_EnemyController> Kamikaze;
}

