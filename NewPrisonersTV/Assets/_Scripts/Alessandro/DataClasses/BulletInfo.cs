using UnityEngine;
[System.Serializable]
public class BulletInfo
{
    public BULLETTYPE bulletType;
    public GameObject bullet;

    public bool isActive;
    public int numberOfHits;
    public float lifeTime;

    [HideInInspector] public Vector3 dir;
    [HideInInspector] public Vector3 newDir;
    [HideInInspector] public Collider2D col;

    public BulletInfo(BULLETTYPE type, GameObject obj)
    {
        bulletType = type;
        bullet = obj;
        numberOfHits = 0;
        isActive = false;
        dir = bullet.transform.forward;
        col = bullet.GetComponent<Collider2D>();
        lifeTime = 0;
    }
    public BulletInfo(BULLETTYPE type, GameObject obj, int hits, bool state, int timer)
    {
        bulletType = type;
        bullet = obj;
        numberOfHits = hits;
        isActive = state;
        dir = bullet.transform.forward;
        col = bullet.GetComponent<Collider2D>();
        lifeTime = timer;
    }
}
