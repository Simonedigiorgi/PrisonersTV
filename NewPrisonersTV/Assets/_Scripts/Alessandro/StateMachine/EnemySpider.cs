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
       
    }   

    public override IEnumerator Die()
    {
        yield return new WaitForEndOfFrame();

        GMController.instance.playerInfo[enemyMembership].score += m_EnemyStats.points; // add points to player
        GMController.instance.UI.UpdateScoreUI(enemyMembership);//Update score on UI
        if(GMController.instance.GetSpidersCount() == GMController.instance.maxSpiders)  // if the spider count is at max then restart the timer of all spawns to give some time between the kill and the new spawn
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
    }
}
