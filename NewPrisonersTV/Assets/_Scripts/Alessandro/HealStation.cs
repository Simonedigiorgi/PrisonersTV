using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character;

public class HealStation : MonoBehaviour
{
    public float coolDown;
    public int price;
    private float timer;
    private bool avaible = true;
	// Use this for initialization
	void Start ()
    {
        timer = coolDown;
	}

    // Update is called once per frame
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
                player.currentLife++;
                GMController.instance.playerInfo[player.playerNumber].score -= price;
                Debug.Log("cured");
            }
            timer = coolDown;
            avaible = false;
        }
    }


}
