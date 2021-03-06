﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

[CreateAssetMenu(menuName = "Prototype/WeaponList")]
public class WeaponList : ScriptableObject
{
    // possible optimization with array
    public List<Weapon3D> LowGrade;
    public List<Weapon3D> MidGrade;
    public List<Weapon3D> Special;
}

