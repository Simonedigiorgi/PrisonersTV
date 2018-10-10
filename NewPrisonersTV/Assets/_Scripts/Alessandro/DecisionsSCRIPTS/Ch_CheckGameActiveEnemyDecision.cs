using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace AI.Decisions
{
    [CreateAssetMenu(menuName = "StateMachine/Decisions/Enemy/CheckForGameActiveEnemy")]
    public class Ch_CheckGameActiveEnemyDecision: Decision
    {
        public override bool Decide(EnemiesAIStateController controller)
        {
            if (GMController.instance.GetGameStatus() && GMController.instance.gameStart)
                return true;
            else
                return false;
        }
    }
}