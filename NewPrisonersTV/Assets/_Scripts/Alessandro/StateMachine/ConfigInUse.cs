using System;
using UnityEngine;

[System.Serializable]
public class ConfigInUse : IEquatable<ConfigInUse>
{
    [SerializeField] private CharacterControlConfig playerInputConfig;
    [SerializeField] private int controllerNumber;

    public CharacterControlConfig PlayerInputConfig { get { return playerInputConfig; } set { playerInputConfig = value; } }
    public int ControllerNumber { get { return controllerNumber; } set { controllerNumber = value; } }

    public ConfigInUse(CharacterControlConfig playerConf)
    {
        playerInputConfig = playerConf;
        controllerNumber = -100;
    }

    public bool Equals(ConfigInUse other)
    {
        if (other == null)
            return false;

        return String.Equals(this.playerInputConfig, other.playerInputConfig);
    }
}


