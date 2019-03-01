using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Prototype/CharactersStats")]
public class CharacterStats : ScriptableObject
{


    [Header("Character Movement Parameters")]
    [Space(10)]
    public float joypadDeathZone = 0.2f;
    public float speed;
    public float dashPower;
    public float dashTimer;
    public LayerMask groundMask;                                               // Ground mask
    public float groundRadius;                                                 // Ground collider radius
    public float jump;                                                       // Player jump value
    public int extraJumpValue;                                               // How many double jumps
    [Space(10)]
    public int life;


}
