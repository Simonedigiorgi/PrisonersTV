﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace GM.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/GM/StoryLoop")]
    public class StoryLoop : _Action
    {

        public override void Execute(GMStateController controller)
        {
            CheckGame(controller);
        }

        private void CheckGame(GMStateController controller)
        {
            for (int i = 0; i < controller.m_GM.playerInfo.Length; i++)
            {
                if (controller.m_GM.playerInfo[i].PlayerController.isAlive) // move to player later
                {
                    // Player life
                    if (controller.m_GM.playerInfo[i].PlayerController.currentLife <= 0)
                    {
                        controller.m_GM.playerInfo[i].PlayerController.currentLife = 0;
                        controller.m_GM.playerInfo[i].PlayerController.isAlive = false;
                        GMController.instance.UI.SetContinueText(i); // set continue text if needed
                    }
                }

            }   

            // game time countdown, influenced by pause 
            if(GMController.instance.currentGameTime > 0 && GMController.instance.gameStart)
            {
                GMController.instance.currentGameTime -= Time.deltaTime;
            }
            else if (GMController.instance.currentGameTime <= 0)
            {
                GMController.instance.gameStart = false;
                Debug.Log("GameLost");
                Time.timeScale = 0;
            }

            // when the time is right enable the key spawn
            if(GMController.instance.currentGameTime <= GMController.instance.keySpawnTime)
            {
                GMController.instance.SetKeyInGame(true);
            }

            // spawn the key
            if(GMController.instance.GetKeyInGame() && GMController.instance.canSpawnKey)
            {
                GMController.instance.canSpawnKey = false;
                GMController.instance.SlowdownSpawns();
                GameObject key = Instantiate(GMController.instance.key, GMController.instance.keySpawn.position, Quaternion.identity);
                key.SetActive(true);
            }          
        }
    }
}
