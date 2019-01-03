using UnityEngine;
using Sirenix.OdinInspector;
[System.Serializable]
public class TensionBonus
{
    [HideInInspector] public bool isActive; 
    public int barMulti;
    public int barThreshold;
    public BONUSTYPE type;

    [ShowIf("type", BONUSTYPE.NewWeapons)]
    public WeaponList newList;
    [ShowIf("type", BONUSTYPE.NewWeapons)]
    [Range(0, 1)]
    public float lowGradeRate = 0;
    [ShowIf("type", BONUSTYPE.NewWeapons)]
    [Range(0, 1)]
    public float midGradeRate = 0;
    [ShowIf("type", BONUSTYPE.NewWeapons)]
    [Range(0, 1)]
    public float specialGradeRate = 0;

    public TensionBonus()
    {
        isActive = false;
        barMulti = 1;
        barThreshold = 1;
        type = BONUSTYPE.None; 
    }
    public TensionBonus(int level, int threshold, BONUSTYPE bonus)
    {
        isActive = false;
        barMulti = level;
        barThreshold = threshold; 
        type = bonus;
    }
    public TensionBonus(TensionBonus t)
    {
        isActive = false;
        barMulti = t.barMulti;
        barThreshold = t.barThreshold;
        type = t.type;

        if(t.type == BONUSTYPE.NewWeapons)
        {
            newList = t.newList;
            lowGradeRate = t.lowGradeRate;
            midGradeRate = t.midGradeRate;
            specialGradeRate = t.specialGradeRate;
        }
    }
}