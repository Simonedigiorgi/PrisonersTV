using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;
using DG.Tweening;

public class EnemyBat : _EnemyController
{
    public EnemyBat()
    {
        enemyType = ENEMYTYPE.Bats;
    }
 
    protected override void Update()
    {
        base.Update();
        if (startSwoopCR)
        {
            StartCoroutine(Swoop());
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

        GMController.instance.playerInfo[enemyMembership].score += m_EnemyStats.points;
        if(GMController.instance.GetBatsCount() == GMController.instance.maxBats)
        {
            for (int i = 0; i < GMController.instance.enemySpawns.Length; i++)
            {
                GMController.instance.enemySpawns[i].ResetTimer();
            }
        }
        GMController.instance.SubBatsCount();
        
      
        Destroy(gameObject);
    }
}
