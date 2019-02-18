using UnityEngine;
using Character;
public class PlayerInfo
{
    private GameObject player;
    private _CharacterController playerController;
    private Transform playerSpawnPoint;
    private int score;
    private int controllerIndex; // real index of the controller in use

    public GameObject Player { get { return player; } }
    public _CharacterController PlayerController { get { return playerController; } }
    public Transform PlayerSpawnPoint { get { return playerSpawnPoint; } }
    public int Score { get { return score; } set { score = value; } }
    public int ControllerIndex { get { return controllerIndex; } set { controllerIndex = value; } }

    public PlayerInfo(GameObject Player, _CharacterController PlayerController, Transform PlayerSpawnPoint, int Score)
    {
        player = Player;
        playerController = PlayerController;
        playerSpawnPoint = PlayerSpawnPoint;
        score = Score;
    }
}


