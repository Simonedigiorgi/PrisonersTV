﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Enemy/SightKamikaze")]
    public class En_SightKamikaze: _Action
    {
        public override void Execute(EnemiesAIStateController controller)
        {
            Sight(controller);
        }

        public void Sight(EnemiesAIStateController controller)
        {
            if (!controller.m_EnemyController.playerSeen && controller.m_EnemyController.currentViewTimer <= 0)
            { 
                // check distance between players and enemy
                for (int i = 0; i < GMController.instance.playerInfo.Length; i++)
                    controller.m_EnemyController.playerSeenDistance[i] = new TargetDistance(i, (controller.m_EnemyController.thisTransform.position - GMController.instance.playerInfo[i].PlayerController.TargetForEnemies.position).sqrMagnitude);

                System.Array.Sort(controller.m_EnemyController.playerSeenDistance);

                // check if the target is in sight based on the distance (check first the closer one)
                for (int i = 0; i < controller.m_EnemyController.playerSeenDistance.Length; i++)
                {   // if the target is in range and is alive 
                    if (controller.m_EnemyController.playerSeenDistance[i].distance <= (controller.enemyStats.attackView * controller.enemyStats.attackView)
                        && GMController.instance.playerInfo[controller.m_EnemyController.playerSeenDistance[i].targetIndex].PlayerController.isAlive)
                    {
                        for (int y = 0; y < controller.m_EnemyController.raycastEyes.Length; y++)
                        {                       
                            Debug.DrawLine(controller.m_EnemyController.raycastEyes[y].position, GMController.instance.playerInfo[controller.m_EnemyController.playerSeenDistance[i].targetIndex].PlayerController.TargetForEnemies.position, Color.red);
                            if (Physics2D.LinecastNonAlloc(controller.m_EnemyController.raycastEyes[y].position, GMController.instance.playerInfo[controller.m_EnemyController.playerSeenDistance[i].targetIndex].PlayerController.TargetForEnemies.position, controller.m_EnemyController.lineCastHits, controller.enemyStats.obstacleMask) <= 0)
                            {
                                controller.m_EnemyController.playerSeenIndex = controller.m_EnemyController.playerSeenDistance[i].targetIndex;
                                if (!controller.m_EnemyController.agent.isOnOffMeshLink) 
                                    controller.m_EnemyController.playerSeen = true;
                            }
                        }
                    }
                }
                controller.m_EnemyController.currentViewTimer = controller.enemyStats.viewCheckFrequenzy;
            }
        }
    }   
}
