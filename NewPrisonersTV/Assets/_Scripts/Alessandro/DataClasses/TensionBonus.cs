using UnityEngine;
using Sirenix.OdinInspector;
[System.Serializable]
public class TensionBonus
{
    [HideInInspector] public bool isActive;
    public int barLevel;
    public int barThreshold;
    public BONUSTYPE type;

    [ShowIf("type", BONUSTYPE.NewWeapons)]
    public WeaponList newList;
     
    public TensionBonus()
    {
        isActive = false;
        barLevel = 1;
        barThreshold = 1;
        type = BONUSTYPE.None;
    }
    public TensionBonus(int level, int threshold, BONUSTYPE bonus)
    {
        isActive = false;
        barLevel = level;
        barThreshold = threshold; 
        type = bonus;
    }
}