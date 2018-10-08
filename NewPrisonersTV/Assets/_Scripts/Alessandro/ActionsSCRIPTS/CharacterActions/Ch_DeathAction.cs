using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace Character.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Characters/DeathAction")]
    public class Ch_DeathAction : _Action
    {
        public override void Execute(CharacterStateController controller)
        {
            Death(controller);
        }

        public void Death(CharacterStateController controller)
        {
           //Call the Respawn method
           controller.m_CharacterController.PlayerRespawn(GMController.instance.playerInfo[controller.m_CharacterController.playerNumber].playerSpawnPoint);
            
            
        }

    }
}
