using UnityEngine;
using AI;
[System.Serializable]
public class DecalInfo
{
    public BULLETTYPE bulletType;
    public GameObject decal;

    public bool isActive;   
    public float lifeTime;
    public _EnemyController enemyHit;

    public DecalInfo(BULLETTYPE type, GameObject obj)
    {
        bulletType = type;
        decal = obj;
        isActive = false;
        lifeTime = 0;
    }
    
}
