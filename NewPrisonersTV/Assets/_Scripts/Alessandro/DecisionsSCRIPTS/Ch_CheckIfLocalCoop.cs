using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace Character.Decisions
{
    [CreateAssetMenu(menuName = "StateMachine/Decisions/Characters/CheckIfLocalCoop")]
    public class Ch_CheckIfLocalCoop: Decision
    {
        public override bool Decide(CharacterStateController controller)
        {
            if (GMController.instance.currentGameMode == GAMEMODE.LocalCoop_2P || GMController.instance.currentGameMode == GAMEMODE.LocalCoop_4P)
                return true;
            else
                return false;
        }
    }
}