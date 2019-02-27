using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MenuButtonDescription
{
    public string buttonFunctionality;
    public Sprite buttonIcon;
}

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
    public BUTTONS dodgeInput;
    public BUTTONS headbuttInput;

    public bool moveArmWithRightStick;

    [Header("Buttons Icons")]
    [Space(10)]
    public MenuButtonDescription[] buttonDescription;

}
