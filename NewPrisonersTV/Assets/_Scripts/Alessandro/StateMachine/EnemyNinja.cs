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
        shuriken = attackParticle.GetComponent<NinjaShurikenParticle>();
        currentShurikenTimer = m_EnemyStats.ShurikenCooldown;
        currentJumpTimer = m_EnemyStats.ninjaJumpCooldown;
    }   

    public override IEnumerator Die()
    {
        yield return new WaitForEndOfFrame();

        GMController.instance.playerInfo[enemyMembership].score += m_EnemyStats.points; // add points to player
        GMController.instance.UI.UpdateScoreUI(enemyMembership);//Update score on UI
        if(GMController.instance.GetNinjaCount() == GMController.instance.maxNinja)  // if the ninja count is at max then restart the timer of all spawns to give some time between the kill and the new spawn
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
