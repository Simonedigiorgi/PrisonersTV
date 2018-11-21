using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;
using DG.Tweening;

public class EnemyKamikaze : _EnemyController
{
    public EnemyKamikaze()
    {
        enemyType = ENEMYTYPE.Kamikaze;
    }

    private void Start()
    {
        currentExplosionTimer = m_EnemyStats.explosionTimer;
        agent.speed = m_EnemyStats.speed;
        if (patrolPoints.Length == 0)
        {
            patrolPoints = new Transform[GMController.instance.enemyPatrolPoints.Length];
            patrolPoints = GMController.instance.enemyPatrolPoints;            
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player_1"))
        {
            canExplode = true;
        }
    }

    public override IEnumerator Die()
    {
        yield return new WaitForEndOfFrame();

        GMController.instance.playerInfo[enemyMembership].score += m_EnemyStats.points; // add points to player
        GMController.instance.UI.UpdateScoreUI(enemyMembership);//Update score on UI
        if(GMController.instance.GetKamikazeCount() == GMController.instance.maxKamikaze)  // if the kamikaze count is at max then restart the timer of all spawns to give some time between the kill and the new spawn
        {
            for (int i = 0; i < GMController.instance.enemySpawns.Length; i++)
            {
                if(GMController.instance.enemySpawns[i].spawnType == enemyType)
                   GMController.instance.enemySpawns[i].ResetTimer();
            }
        }
        GMController.instance.SubKamikazeCount();
        GMController.instance.allEnemies.Remove(this);     
      
        Destroy(gameObject);
    }
}
