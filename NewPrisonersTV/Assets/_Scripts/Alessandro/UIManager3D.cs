using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Character;
public class UIManager3D : MonoBehaviour {

    public Text player1Continue;
    public Text player2Continue;

    Text hammoP1;                                                                                   //UI player1 hammo text
    Text hammoP2;                                                                                   //UI player2 hammo text

    Text scoreP1;                                                                                   //UI player1 score text
    Text scoreP2;                                                                                   //UI player2 score text

    private Transform player1;                                                                      //Player1 transform
    private Transform player2;                                                                      //Player2 transform
    _CharacterController pc1;                                                                           //PlayerController1
    _CharacterController pc2;                                                                           //Playercontroller2

    GameObject handP1;                                                                              //Hand player1
    GameObject handP2;                                                                              //Hand player2

    Weapon3D actualWeaponP1;                                                                          //the weapon grabbed on P1
    Weapon3D actualWeaponP2;                                                                          //the weapon grabbed on p2

    SpriteRenderer lifeBarP1;                                                                       //the lifeBar on player1
    SpriteRenderer lifeBarP2;                                                                       //the lifeBar on player2

    Camera mainCamera;

    GameManager gameManager;

    [Tooltip("The horizontal distance of the UI hammo text to the player")] public float hammoHorizontalOffset;
    [Tooltip("The vertical distance of the UI hammo text to the player")] public float hammoVerticalOffset;

    LifeController3D life1, life2;                              // Get the Player_1 $$ Player_2 LifeController script component

    void Start ()
    {
        player1 = GameObject.FindGameObjectWithTag("Player_1").transform;
        pc1 = player1.GetComponent<_CharacterController>();//test
        handP1 = GameObject.Find("Hand_Player1");
        hammoP1 = transform.GetChild(0).GetChild(1).GetComponent<Text>();
        lifeBarP1 = player1.GetChild(4).GetComponent<SpriteRenderer>();
        scoreP1 = transform.GetChild(0).GetChild(2).GetComponent<Text>();

        life1 = player1.GetComponent<LifeController3D>();

        player2 = GameObject.FindGameObjectWithTag("Player_2").transform;
        pc2 = player2.GetComponent<_CharacterController>();
        handP2 = GameObject.Find("Hand_Player2");
        hammoP2 = transform.GetChild(1).GetChild(1).GetComponent<Text>();
        lifeBarP2 = player2.GetChild(4).GetComponent<SpriteRenderer>();
        scoreP2 = transform.GetChild(1).GetChild(2).GetComponent<Text>();

        life2 = player2.GetComponent<LifeController3D>();

        mainCamera = Camera.main;

        //Find game manager
        if (GameObject.Find("GameManager") != null)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        else
        {
            Debug.Log("ADD GAME MANAGER TO SCENE!!! (named 'GameManager')");
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        #region ContinueText
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
#endregion

        #region Bullets

        //Switch weapon
        if(actualWeaponP1 == null && handP1.transform.childCount > 0)
        {
            actualWeaponP1 = handP1.transform.GetChild(0).GetComponent<Weapon3D>();
        }

        if (actualWeaponP2 == null && handP2.transform.childCount > 0)
        {
            actualWeaponP2 = handP2.transform.GetChild(0).GetComponent<Weapon3D>();
        }

        //Enabled and disabled Hammo text and assign hammo value at the text
        if (handP1.transform.childCount <= 0)
        {
            hammoP1.text = 0.ToString();
            hammoP1.gameObject.SetActive(false);
        }
        else
        {
            hammoP1.gameObject.SetActive(true);
            SetBulletsText(1);
        }

        if (handP2.transform.childCount <= 0)
        {
            hammoP2.text = 0.ToString();
            hammoP2.gameObject.SetActive(false);
        }
        else
        {
            hammoP2.gameObject.SetActive(true);
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
        #endregion

        #region Life
        
        //Rescale and Recolor life bar
        //P1
        if (life1.life == 3)
        {
            lifeBarP1.transform.localScale = new Vector3(15, 2.5f, 0);
            lifeBarP1.color = Color.green;
        }
        else if(life1.life == 2)
        {
            lifeBarP1.transform.localScale = new Vector3(10, 2.5f, 0);
            lifeBarP1.color = Color.yellow;
        }
        else if (life1.life == 1)
        {
            lifeBarP1.transform.localScale = new Vector3(5, 2.5f, 0);
            lifeBarP1.color = Color.red;
        }
        else if (life1.life <= 0)
        {
            lifeBarP1.transform.localScale = Vector3.zero;
        }

        //P2
        if (life2.life == 3)
        {
            lifeBarP2.transform.localScale = new Vector3(15, 2.5f, 0);
            lifeBarP2.color = Color.green;
        }
        else if (life2.life == 2)
        {
            lifeBarP2.transform.localScale = new Vector3(10, 2.5f, 0);
            lifeBarP2.color = Color.yellow;
        }
        else if (life2.life == 1)
        {
            lifeBarP2.transform.localScale = new Vector3(5, 2.5f, 0);
            lifeBarP2.color = Color.red;
        }
        else if (life2.life <= 0)
        {
            lifeBarP2.transform.localScale = Vector3.zero;
        }
        #endregion

        #region Score

        scoreP1.text = "P1 Score: " + gameManager.P1Score.ToString();
        scoreP2.text = "P2 Score: " + gameManager.P2Score.ToString();

#endregion
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
