using UnityEngine;
[System.Serializable]
public class DecalList
{
    public DECALTYPE decalType;
    public GameObject decal;
    public DecalList(DECALTYPE type, GameObject obj)
    {
        decalType = type;
        decal = obj;
    }
}
