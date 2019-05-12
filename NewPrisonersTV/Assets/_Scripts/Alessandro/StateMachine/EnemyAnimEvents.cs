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

    public void Death()
    {
        controller.startDieCoroutine = true;
    }

    //public void StartAggro()
    //{
    //   controller.isAggroAnim = true;
    //    Debug.Log("ENTER "+controller.gameObject.name);
    //}

    public void EndAggro()
    {
        controller.isAggroAnim = false;
        Debug.Log("EXIT " + controller.gameObject.name);
    }
    
}
