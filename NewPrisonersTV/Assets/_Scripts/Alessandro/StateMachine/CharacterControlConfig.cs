using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Prototype/CharacterControlConfig")]
public class CharacterControlConfig : ScriptableObject
{
    [Header("Character Buttons")]
    [Space(10)]
    public TYPEOFINPUT controller;
    public HORIZONTAL LeftHorizontal;                                          
    public VERTICAL LeftVertical;                                          
    public HORIZONTAL RightHorizontal;                                         
    public VERTICAL RightVertical;                                            
    public BUTTONS shootInput;                         
    public BUTTONS jumpInput;                          
    public BUTTONS interactInput; 
    public BUTTONS respawnInput;
    public BUTTONS pauseInput;
    public bool moveArmWithRightStick;

}
