using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;
using DG.Tweening;
using Character;

public class EnemySentinel : _EnemyController
{
    public EnemySentinel()
    {
        enemyType = ENEMYTYPE.Sentinel;
    }

    protected override void Start()
    {  //assign start directions
        if (m_EnemyStats.myStartDirection == STARTDIRECTION.Right)
        {
            direction = 1; 
        }
        else
        {
            direction = -1;
        }
        RotateTowardDirection(thisTransform, direction); 
        bullet = attackParticle.GetComponent<EnemyBulletParticle>();
        currentBulletTimer = m_EnemyStats.bulletCooldown;
        currentBarrageShots = m_EnemyStats.numBarrageShots;
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            direction *= -1;
            RotateTowardDirection(thisTransform, direction);
        }
    }
    protected override void Update()
    {
        base.Update();
        if (GMController.instance.gameStart)
        {
            if(canBarrageCR)
            {
                StartCoroutine(Barrage());
            }
        }
    }

    public override IEnumerator Die()
    {
        yield return new WaitForEndOfFrame();

        GMController.instance.playerInfo[enemyOwnership].Score += (m_EnemyStats.points * GMController.instance.tensionStats.scoreMultiXLevel[GMController.instance.currentTensionMulti - 1]); // add points to player
        GMController.instance.UI.UpdateScoreUI(enemyOwnership);//Update score on UI
        GMController.instance.TensionThresholdCheck(GMController.instance.tensionStats.enemyKillPoints);// add tension
        if (GMController.instance.GetSentinelCount() == GMController.instance.maxBats)  // if the sentinel count is at max then restart the timer of all spawns to give some time between the kill and the new spawn
        {
            for (int i = 0; i < GMController.instance.enemySpawns.Length; i++)
            {
                if (GMController.instance.enemySpawns[i].spawnType == enemyType)
                    GMController.instance.enemySpawns[i].ResetTimer();
            }
        }
        GMController.instance.SubSentinelCount();
        GMController.instance.allEnemies.Remove(this);     
      
        Destroy(gameObject);
        yield return null;
    }
}
