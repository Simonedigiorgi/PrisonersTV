using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace Character.Decisions
{
    [CreateAssetMenu(menuName = "StateMachine/Decisions/GM/CheckIfMenu")]
    public class Ch_CheckIfMenu: Decision
    {
        public override bool Decide(CharacterStateController controller)
        {
            if (GMController.instance.GetGameMode() == GAMEMODE.None)
                return true;
            else
                return false;
        }
    }
}