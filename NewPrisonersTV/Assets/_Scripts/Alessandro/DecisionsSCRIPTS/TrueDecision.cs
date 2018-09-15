using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

[CreateAssetMenu(menuName = "StateMachine/Decisions/TrueDecision")]
public class TrueDecision : Decision {

    public override bool Decide(CharacterStateController controller) { return true; }

    //public override bool Decide(EnemiesAIStateController controller) { return true; }

    public override bool Decide(GMStateController controller) { return true; }
}
