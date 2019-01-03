using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class EnemySpider : _EnemyController
{
    public EnemySpider()
    {
        enemyType = ENEMYTYPE.Spider;
    }

    protected override void Start()
    {
        base.Start();
        currentPathTimer = m_EnemyStats.pathCheckFrequenzy;
    }

    public override IEnumerator Die() 
    {
        yield return new WaitForEndOfFrame();

        GMController.instance.playerInfo[enemyMembership].score += (m_EnemyStats.points * GMController.instance.tensionStats.scoreMultiXLevel[GMController.instance.currentTensionMulti - 1]); // add points to player
        GMController.instance.UI.UpdateScoreUI(enemyMembership);//Update score on UI
        GMController.instance.TensionThresholdCheck(GMController.instance.tensionStats.enemyKillPoints);// add tension
        if (GMController.instance.GetSpidersCount() == GMController.instance.maxSpiders)  // if the spider count is at max then restart the timer of all spawns to give some time between the kill and the new spawn
        {
            for (int i = 0; i < GMController.instance.enemySpawns.Length; i++)
            {
                if(GMController.instance.enemySpawns[i].spawnType == enemyType)
                   GMController.instance.enemySpawns[i].ResetTimer();
            }
        }
        GMController.instance.SubSpidersCount();
        GMController.instance.allEnemies.Remove(this);
        Destroy(gameObject);
        yield return null;
    }
}
