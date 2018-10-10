using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace Character.Decisions
{
    [CreateAssetMenu(menuName = "StateMachine/Decisions/Characters/CheckForGameActivePlayer")]
    public class Ch_CheckGameActivePlayerDecision: Decision
    {
        public override bool Decide(CharacterStateController controller)
        {
            if (GMController.instance.GetGameStatus() && GMController.instance.gameStart)
                return true;
            else
                return false;
        }
    }
}