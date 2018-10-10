using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace Character.Decisions
{
    [CreateAssetMenu(menuName = "StateMachine/Decisions/GM/CheckIfSurvival")]
    public class Ch_CheckIfSurvival: Decision
    {
        public override bool Decide(GMStateController controller)
        {
            if (GMController.instance.GetGameMode() == GAMEMODE.Survival)            
                return true;           
            else
                return false;
        }
    }
}