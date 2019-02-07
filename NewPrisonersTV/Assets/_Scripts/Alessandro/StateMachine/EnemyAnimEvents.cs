using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class EnemyAnimEvents : MonoBehaviour
{
    public _EnemyController controller;

    public void Explode() // kamikaze
    {
        controller.explosionParticle.Explosion(controller.attackSpawn.position);
        controller.startDieCoroutine = true;
    }
}
