using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace Character.Decisions
{
    [CreateAssetMenu(menuName = "StateMachine/Decisions/GM/CheckIfStory")]
    public class Ch_CheckIfStory: Decision
    {
        public override bool Decide(GMStateController controller)
        {
            if (GMController.instance.GetGameMode() == GAMEMODE.Story) 
                return true;           
            else
                return false;
        }
    }
}