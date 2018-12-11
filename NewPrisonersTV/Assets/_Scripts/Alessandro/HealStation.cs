using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character;

public class HealStation : MonoBehaviour
{
    public float coolDown;
    public int price;
    [Range(1,100)]public int healAmountPercentage;

    private float timer;
    private bool avaible = true;
    private int healAmmount;
	void Start ()
    {
        timer = coolDown;
	}

    void Update ()
    {
        if (GMController.instance.gameStart)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                avaible = true;
            }
        }       
	}

    public void UseStation(_CharacterController player)
    {
        if (avaible && GMController.instance.playerInfo[player.playerNumber].score >= price)
        { 
            if (player.currentLife < player.m_CharStats.life)
            {
                healAmmount = healAmountPercentage * player.m_CharStats.life / 100;
                player.currentLife += healAmountPercentage;
                if (player.currentLife > player.m_CharStats.life)
                    player.currentLife = player.m_CharStats.life;
                Debug.Log(healAmmount + "  " + player.currentLife);
                GMController.instance.playerInfo[player.playerNumber].score -= price; // subtract payment from score
                GMController.instance.UI.UpdateScoreUI(player.playerNumber); // update score on UI
                GMController.instance.UI.UpdateLifeUI(player.playerNumber);// update life on UI
                Debug.Log("cured");
            }
            timer = coolDown;
            avaible = false;
        }
    }


}
