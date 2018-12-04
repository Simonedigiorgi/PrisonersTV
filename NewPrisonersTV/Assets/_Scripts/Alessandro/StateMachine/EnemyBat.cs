using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;
using DG.Tweening;
using Character;

public class EnemyBat : _EnemyController
{
    public EnemyBat()
    {
        enemyType = ENEMYTYPE.Bat;
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
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        //turn the direction if collide on wall
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Enemy"))
        {
            direction *= -1;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // When player trigger an enemy
        if (collision.CompareTag("Player_1"))
        {
            _CharacterController player = collision.GetComponent<_CharacterController>();
            player.currentLife -= m_EnemyStats.attackValue;
            if (player.currentLife <= 0)
                player.currentLife = 0;
            GMController.instance.UI.UpdateLifeUI(player.playerNumber); // update life on UI
        }
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
        GMController.instance.UI.UpdateScoreUI(enemyMembership);//Update score on UI
        if(GMController.instance.GetBatsCount() == GMController.instance.maxBats)  // if the bat count is at max then restart the timer of all spawns to give some time between the kill and the new spawn
        {
            for (int i = 0; i < GMController.instance.enemySpawns.Length; i++)
            {
                if (GMController.instance.enemySpawns[i].spawnType == enemyType)
                    GMController.instance.enemySpawns[i].ResetTimer();
            }
        }
        GMController.instance.SubBatsCount();
        GMController.instance.allEnemies.Remove(this);     
      
        Destroy(gameObject);
        yield return null;
    }
}
