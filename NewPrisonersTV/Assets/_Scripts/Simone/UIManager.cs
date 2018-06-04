using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Text player1Continue;
    public Text player2Continue;

    Text hammoP1;                                                                                   //UI player1 hammo text
    Text hammoP2;                                                                                   //UI player2 hammo text

    private Transform player1;                                                                      //Player1 transform
    private Transform player2;                                                                      //Player2 transform
    PlayerController pc1;                                                                           //PlayerController1
    PlayerController pc2;                                                                           //Playercontroller2

    GameObject handP1;                                                                              //Hand player1
    GameObject handP2;                                                                              //Hand player2

    bool P1GetWeapon = false;                                                                       //check if the player1 has get weapon
    bool P2GetWeapon = false;                                                                       //check if the player2 has get weapon

    Weapon actualWeaponP1;                                                                          //the weapon grabbed on P1
    Weapon actualWeaponP2;                                                                          //the weapon grabbed on p2

    Camera mainCamera;

    [Tooltip("The horizontal distance of the UI hammo text to the player")] public float hammoHorizontalOffset;
    [Tooltip("The vertical distance of the UI hammo text to the player")] public float hammoVerticalOffset;

    void Start ()
    {
        player1 = GameObject.FindGameObjectWithTag("Player_1").transform;
        pc1 = player1.GetComponent<PlayerController>();
        handP1 = GameObject.Find("Hand_Player1");
        hammoP1 = transform.GetChild(0).GetChild(1).GetComponent<Text>();

        player2 = GameObject.FindGameObjectWithTag("Player_2").transform;
        pc2 = player2.GetComponent<PlayerController>();
        handP2 = GameObject.Find("Hand_Player2");
        hammoP2 = transform.GetChild(1).GetChild(1).GetComponent<Text>();

        mainCamera = Camera.main;
    }
	
	// Update is called once per frame
	void Update () {

        //Enabled and disabled Continue text
        if (!player1.gameObject.activeSelf)
        {
            player1Continue.enabled = true;

        }
        else
        {
            player1Continue.enabled = false;
        }

        if (!player2.gameObject.activeSelf)
        {
            player2Continue.enabled = true;

        }
        else
        {
            player2Continue.enabled = false;
        }

        //Enabled and disabled Hammo text and assign hammo value at the text
        if (handP1.transform.childCount <= 0)
        {
            P1GetWeapon = false;
            hammoP1.text = 0.ToString();
            hammoP1.gameObject.SetActive(false);
        }
        else
        {
            hammoP1.gameObject.SetActive(true);

            if (!P1GetWeapon)
            {
                P1GetWeapon = true;
                actualWeaponP1 = handP1.transform.GetChild(0).GetComponent<Weapon>();
            }

            SetBulletsText(1);
        }

        if (handP2.transform.childCount <= 0)
        {
            P2GetWeapon = false;
            hammoP2.text = 0.ToString();
            hammoP2.gameObject.SetActive(false);
        }
        else
        {
            hammoP2.gameObject.SetActive(true);

            if (!P2GetWeapon)
            {
                P2GetWeapon = true;
                actualWeaponP2 = handP2.transform.GetChild(0).GetComponent<Weapon>();
            }

            SetBulletsText(2);
        }

        //set hammo text position
        hammoP1.transform.position = mainCamera.WorldToScreenPoint(player1.transform.position);
        if (pc1.facingRight)
            hammoP1.transform.position += new Vector3(hammoHorizontalOffset, hammoVerticalOffset, 0);
        else
            hammoP1.transform.position += new Vector3(-hammoHorizontalOffset, hammoVerticalOffset, 0);

        hammoP2.transform.position = mainCamera.WorldToScreenPoint(player2.transform.position);
        if (pc2.facingRight)
            hammoP2.transform.position += new Vector3(hammoHorizontalOffset, hammoVerticalOffset, 0);
        else
            hammoP2.transform.position += new Vector3(-hammoHorizontalOffset, hammoVerticalOffset, 0);
    }

    //use this for change ui hammo value
    public void SetBulletsText(int player)
    {
        if(player == 1)
        {
            hammoP1.text = actualWeaponP1.bullets.ToString();
        }
        else if(player == 2)
        {
            hammoP2.text = actualWeaponP2.bullets.ToString();
        }
    }
}
