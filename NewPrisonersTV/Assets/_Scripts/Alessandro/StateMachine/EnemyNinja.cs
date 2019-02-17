using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class EnemyNinja : _EnemyController
{
    public EnemyNinja()
    {
        enemyType = ENEMYTYPE.Ninja;
    }

    protected override void Start()
    {
        base.Start();
        bullet = attackParticle.GetComponent<EnemyBulletParticle>();
        currentBulletTimer = m_EnemyStats.bulletCooldown;
        currentJumpTimer = m_EnemyStats.ninjaJumpCooldown;
    }   

    public override IEnumerator Die()
    {
        yield return new WaitForEndOfFrame();

        GMController.instance.playerInfo[enemyOwnership].Score += (m_EnemyStats.points * GMController.instance.tensionStats.scoreMultiXLevel[GMController.instance.currentTensionMulti - 1]); // add points to player
        GMController.instance.UI.UpdateScoreUI(enemyOwnership);//Update score on UI
        GMController.instance.TensionThresholdCheck(GMController.instance.tensionStats.enemyKillPoints);// add tension
        if (GMController.instance.GetNinjaCount() == GMController.instance.maxNinja)  // if the ninja count is at max then restart the timer of all spawns to give some time between the kill and the new spawn
        {
            for (int i = 0; i < GMController.instance.enemySpawns.Length; i++)
            {
                if(GMController.instance.enemySpawns[i].spawnType == enemyType)
                   GMController.instance.enemySpawns[i].ResetTimer();
            }
        }
        GMController.instance.SubNinjaCount();
        GMController.instance.allEnemies.Remove(this);     
      
        Destroy(gameObject);
        yield return null;
    }
}
