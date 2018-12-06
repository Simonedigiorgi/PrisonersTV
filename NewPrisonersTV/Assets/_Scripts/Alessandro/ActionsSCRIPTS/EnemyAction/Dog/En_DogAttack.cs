﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using Character;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/DogAttack")]
    public class En_DogAttack : _Action
    {
        public override void Execute(EnemiesAIStateController controller) 
        {
            Attack(controller);
        }

        public void Attack(EnemiesAIStateController controller)
        {
            // set the player as destination---------------------------------------------------------
            controller.m_EnemyController.agent.destination = GMController.instance.playerInfo[controller.m_EnemyController.playerSeenIndex].player.transform.position;

            //change agent speed if is on NavLink----------------------------------------------------
            if (controller.m_EnemyController.agent.isOnOffMeshLink)
                controller.m_EnemyController.agent.speed = controller.enemyStats.jumpSpeed;
            // if the agent is closer than the stopping distance then stops itself
            else if (!controller.m_EnemyController.agent.isOnOffMeshLink && controller.m_EnemyController.agent.remainingDistance > controller.m_EnemyController.agent.stoppingDistance)
                controller.m_EnemyController.agent.speed = controller.enemyStats.runSpeed;
            else
            {
               // controller.m_EnemyController.agent.speed = 0; 
                controller.m_EnemyController.agent.velocity = Vector3.zero;
            }
            //---------------------------------------------------------------------------------------
            // if the player is in reach the agent will attack 
            float biteDistance = (controller.m_EnemyController.thisTransform.position - GMController.instance.playerInfo[controller.m_EnemyController.playerSeenIndex].player.transform.position).sqrMagnitude;
            if (biteDistance <= (controller.enemyStats.triggerBiteDistance * controller.enemyStats.triggerBiteDistance) && controller.m_EnemyController.currentBiteTimer <= 0)
            {
                controller.m_EnemyController.enemyAnim.SetTrigger("Bite"); 
                Collider2D hit = Physics2D.OverlapCircle(controller.m_EnemyController.attackSpawn.position, controller.enemyStats.biteRadius, controller.enemyStats.hitMask);
                if (hit != null)
                {
                    controller.m_EnemyController.DamagePlayer(hit);                  
                }
                controller.m_EnemyController.currentBiteTimer = controller.enemyStats.biteCooldown;

            }
          
        }   

    }
}
