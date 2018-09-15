using UnityEngine;
using Character;
public class PlayerInfo
{
    public GameObject player;
    public _CharacterController playerController;
    public Transform playerSpawnPoint;
    public int score;

    public PlayerInfo(GameObject Player, _CharacterController PlayerController, Transform PlayerSpawnPoint, int Score)
    {
        player = Player;
        playerController = PlayerController;
        playerSpawnPoint = PlayerSpawnPoint;
        score = Score;
    }
}


