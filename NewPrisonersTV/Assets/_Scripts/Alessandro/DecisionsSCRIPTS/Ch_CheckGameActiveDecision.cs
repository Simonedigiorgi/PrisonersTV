using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace Character.Decisions
{
    [CreateAssetMenu(menuName = "StateMachine/Decisions/Characters/CheckForGameActive")]
    public class Ch_CheckGameActiveDecision: Decision
    {
        public override bool Decide(CharacterStateController controller)
        {
            return GMController.instance.GetGameStatus(); ;
        }
    }
}