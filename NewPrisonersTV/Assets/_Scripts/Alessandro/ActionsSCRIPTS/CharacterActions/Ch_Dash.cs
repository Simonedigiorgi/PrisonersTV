using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace Character.Actions
{
    [CreateAssetMenu(menuName = "StateMachine/Actions/Characters/Dash")]
    public class Ch_Dash : _Action
    {
        public override void Execute(CharacterStateController controller)
        {
            DashAction(controller);
        }

        public void DashAction(CharacterStateController controller)
        {
            controller.m_CharacterController.isInDash = true;
            controller.m_CharacterController.playerAnim.SetBool("Dash",true);
            // perform dash without the gravity (PowerUp)
            //if (powerDash)
            //{
            //    rb.gravityScale = 0;
            //}

            if (controller.m_CharacterController.facingRight)
                controller.m_CharacterController.rb.velocity = Vector2.left * controller.characterStats.dashPower;
            else
                controller.m_CharacterController.rb.velocity = Vector2.right * controller.characterStats.dashPower;

        }
    }
}
