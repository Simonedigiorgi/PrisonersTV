using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

[CreateAssetMenu(menuName = "Prototype/BulletList")]
public class BulletList : ScriptableObject
{
    public List<ParticleEmitterRaycastBullet> bulletTypes;
}

