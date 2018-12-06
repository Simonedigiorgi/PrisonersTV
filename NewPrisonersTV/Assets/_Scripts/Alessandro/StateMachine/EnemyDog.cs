using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class EnemyDog : _EnemyController
{
    public EnemyDog()
    {
        enemyType = ENEMYTYPE.Dog; 
    }

    protected override void Start()
    {
        base.Start();
        path = new UnityEngine.AI.NavMeshPath(); 
        currentBiteTimer = m_EnemyStats.biteCooldown;
        currentDisengageTimer = m_EnemyStats.disengageTimer;
    }

    public override IEnumerator Die() 
    {
        yield return new WaitForEndOfFrame();

        GMController.instance.playerInfo[enemyMembership].score += m_EnemyStats.points; // add points to player
        GMController.instance.UI.UpdateScoreUI(enemyMembership);//Update score on UI
        if(GMController.instance.GetDogsCount() == GMController.instance.maxDogs)  // if the dog count is at max then restart the timer of all dog spawns to give some time between the kill and the new spawn
        {
            for (int i = 0; i < GMController.instance.enemySpawns.Length; i++)
            {
                if(GMController.instance.enemySpawns[i].spawnType == enemyType)
                   GMController.instance.enemySpawns[i].ResetTimer();
            }
        }
        GMController.instance.SubDogsCount();
        GMController.instance.allEnemies.Remove(this);
        Destroy(gameObject);
        yield return null;
    }
}
