using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;
using DG.Tweening;

public class EnemyBat : _EnemyController
{
    public EnemyBat()
    {
        enemyType = ENEMYTYPE.Bat;
    }
 
    protected override void Update()
    {
        if (GMController.instance.gameStart)
        {
            base.Update();
            if (startSwoopCR)
            {
                StartCoroutine(Swoop());
            }
        }
    }

    public IEnumerator Swoop()
    {
        startSwoopCR = false;
        swoopCoroutineInExecution = true;
        transform.DOMove(endSwoopPosition, m_EnemyStats.swoopMoreSlowly, false);
        yield return new WaitUntil(() => transform.position == endSwoopPosition);
  
        transform.DOMove(startSwoopPosition, m_EnemyStats.swoopMoreSlowly, false);
        yield return new WaitUntil(() => transform.position == startSwoopPosition);
    
        playerSeen = false;
        swoopCoroutineInExecution = false;
    }
    public override IEnumerator Die()
    {
        yield return new WaitForEndOfFrame();

        GMController.instance.playerInfo[enemyMembership].score += m_EnemyStats.points; // add points to player
        if(GMController.instance.GetBatsCount() == GMController.instance.maxBats)  // if the bat count is at max then restart the timer of all spawns to give some time between the kill and the new spawn
        {
            for (int i = 0; i < GMController.instance.enemySpawns.Length; i++)
            {
                GMController.instance.enemySpawns[i].ResetTimer();
            }
        }
        GMController.instance.SubBatsCount();
        GMController.instance.allEnemies.Remove(this);
        
      
        Destroy(gameObject);
    }
}
