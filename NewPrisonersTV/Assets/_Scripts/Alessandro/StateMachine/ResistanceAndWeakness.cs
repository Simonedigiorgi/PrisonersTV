using UnityEngine;

[System.Serializable]
public class ResistanceAndWeakness
{
    public DAMAGETYPE type;
    public int value;

    public ResistanceAndWeakness(DAMAGETYPE t, int v)
    {
        type = t;
        value = v;
    }
}
