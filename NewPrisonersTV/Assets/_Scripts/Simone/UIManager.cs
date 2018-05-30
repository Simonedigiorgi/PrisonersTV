using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Text player1Continue;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (GameObject.FindGameObjectWithTag("Player_1") == null)
        {
            player1Continue.enabled = true;

        }
        else
        {
            player1Continue.enabled = false;
        }
    }
}
