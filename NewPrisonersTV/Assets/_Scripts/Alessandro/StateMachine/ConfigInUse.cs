using System;
using UnityEngine;

[System.Serializable]
public class ConfigInUse : IEquatable<ConfigInUse>
{
    [SerializeField] private CharacterControlConfig playerInputConfig;
    [SerializeField] private int controllerIndex; // real index in the unity static array of the controller in use 
    [SerializeField] private int controllerNumber;// real real order of the controller attached, used to mantain control of the selected player
    [SerializeField] private TYPEOFINPUT lastUsed;// track the last type of input controller used, useful after transitions trough levels

    private int defaultNumber = -100;

    public CharacterControlConfig PlayerInputConfig { get { return playerInputConfig; } set { playerInputConfig = value; } }
    public int ControllerIndex { get { return controllerIndex; } set { controllerIndex = value; } }
    public int ControllerNumber { get { return controllerNumber; } set { controllerNumber = value; } }
    public TYPEOFINPUT LastUsed { get { return lastUsed; } set { lastUsed = value; } }
    public int DefaultNumber { get { return defaultNumber; } }

    public ConfigInUse(CharacterControlConfig playerConf)
    {
        playerInputConfig = playerConf;
        controllerIndex = defaultNumber;
        controllerNumber = defaultNumber;
        lastUsed = TYPEOFINPUT.J;
    } 

    public bool Equals(ConfigInUse other)
    {
        if (other == null)
            return false;

        return String.Equals(this.playerInputConfig, other.playerInputConfig);
    }
}


