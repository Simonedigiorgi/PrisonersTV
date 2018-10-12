using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Prototype/CharacterControlConfig")]
public class CharacterControlConfig : ScriptableObject
{
    [Header("Character Buttons")]
    [Space(10)]
    public HORIZONTAL LeftHorizontal;                                           // Get Horiziontal
    public VERTICAL LeftVertical;                                             // Get Vertical
    public HORIZONTAL RightHorizontal;                                           // Get Horiziontal
    public VERTICAL RightVertical;                                             // Get Vertical
    public BUTTONS shootInput;                         // Player1_Button X || Player2_Button X (Shoot with you weapon)
    public BUTTONS jumpInput;                          // Player1_Button A || Player2_Button A
    public BUTTONS interactInput; 
    public BUTTONS respawnInput;
    public BUTTONS pauseInput;
    public bool moveArmWithRightStick;
}
