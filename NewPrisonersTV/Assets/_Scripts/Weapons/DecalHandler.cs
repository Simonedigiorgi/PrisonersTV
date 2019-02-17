using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class DecalHandler : MonoBehaviour
{
    public BULLETTYPE type;
    public int poolSize;
    public DecalInfo[] decalList;
    public float decalLifeTime;

    private int playerNumber;
    public Weapon3D playerWeapon = null;
    private bool poolReady = false;

    private void Start()
    {
        playerNumber = transform.parent.GetSiblingIndex();
        PoolFilling();
    }

    private void Update()
    {
        if (GMController.instance.gameStart && poolReady)
        {
            // get the player weapon if needed
            if (GMController.instance.playerInfo[playerNumber].PlayerController.currentWeapon != null &&
                GMController.instance.playerInfo[playerNumber].PlayerController.currentWeapon.decalType == type &&
                playerWeapon == null)
            {
                playerWeapon = GMController.instance.playerInfo[playerNumber].PlayerController.currentWeapon;
            }

            // assign this depot to the weapon
            if (playerWeapon != null)            
                if (playerWeapon.currentDecal != this)                
                    playerWeapon.currentDecal = this;
        
            for (int i = 0; i < decalList.Length; i++)
            {
                if (decalList[i].isActive)
                {
                    decalList[i].lifeTime -= Time.deltaTime;

                    if ((decalList[i].enemyHit != null && decalList[i].enemyHit.currentLife <= 0) ||
                        decalList[i].lifeTime <= 0)
                    {
                        Collector(i);
                    }
                }
            }
        }
    }

    public void PlaceDecal(Transform bulletHit, _EnemyController enemyHit)
    {
        int decalChosen = CheckForDecal();

        // set the decal to be used again and remove it from the previous enemy (if there was one)
        if (decalList[decalChosen].enemyHit != null)
            decalList[decalChosen].enemyHit.decalsNum--;
        // place the decal
        decalList[decalChosen].lifeTime = decalLifeTime;
        decalList[decalChosen].decal.transform.position = bulletHit.position;
        decalList[decalChosen].decal.transform.rotation = Quaternion.LookRotation(bulletHit.forward, bulletHit.up);
        decalList[decalChosen].enemyHit = enemyHit;
        decalList[decalChosen].decal.transform.parent = enemyHit.transform;
        enemyHit.hasDecalsOn = true;
        enemyHit.decalsNum++;
        decalList[decalChosen].isActive = true;
    }
    public void PlaceDecal(Transform bulletHit)
    {
        int decalChosen = CheckForDecal();

        // set the decal to be used again
        if (decalList[decalChosen].enemyHit != null)
            decalList[decalChosen].enemyHit.decalsNum--;
        decalList[decalChosen].decal.transform.parent = null;
        // place the decal
        decalList[decalChosen].decal.transform.position = bulletHit.position;
        decalList[decalChosen].decal.transform.rotation = Quaternion.LookRotation(bulletHit.forward, bulletHit.up);
        decalList[decalChosen].lifeTime = decalLifeTime;
        decalList[decalChosen].isActive = true;
    }
    public void PlaceDecal(RaycastHit2D hit, Vector3 velocity, _EnemyController enemyHit)
    {
        int decalChosen = CheckForDecal();

        // set the decal to be used again and remove it from the previous enemy (if there was one)
        if (decalList[decalChosen].enemyHit != null)
            decalList[decalChosen].enemyHit.decalsNum--;
        // place the decal
        decalList[decalChosen].lifeTime = decalLifeTime;
        decalList[decalChosen].decal.transform.position = hit.point;
        decalList[decalChosen].decal.transform.rotation = Quaternion.LookRotation(velocity.normalized);
        decalList[decalChosen].enemyHit = enemyHit;
        decalList[decalChosen].decal.transform.parent = enemyHit.transform;
        enemyHit.hasDecalsOn = true;
        enemyHit.decalsNum++;
        decalList[decalChosen].isActive = true;
    }
    public void PlaceDecal(RaycastHit2D hit, Vector3 velocity)
    {
        int decalChosen = CheckForDecal();

        // set the decal to be used again
        if (decalList[decalChosen].enemyHit != null)
            decalList[decalChosen].enemyHit.decalsNum--;
        decalList[decalChosen].decal.transform.parent = null;
        // place the decal
        decalList[decalChosen].decal.transform.position = hit.point;
        decalList[decalChosen].decal.transform.rotation = Quaternion.LookRotation(velocity.normalized);
        decalList[decalChosen].lifeTime = decalLifeTime;
        decalList[decalChosen].isActive = true;
    }

    private int CheckForDecal()
    {
        bool placed = false;
        int decalChosen = 0;
        for (int i = 0; i < decalList.Length; i++)
        {
            if (!decalList[i].isActive)
            {
                decalChosen = i;
                placed = true;
                break;
            }
        }
        if (!placed)
        {
            for (int i = 0; i < decalList.Length; i++)
            {
                if (decalList[i].lifeTime <= decalList[decalChosen].lifeTime)
                    decalChosen = i;
            }
        }
        return decalChosen;
    }

    private void PoolFilling()
    {
        DecalInfo decal = null;
        for (int i = 0; i < GMController.instance.decalDepot.depot.Length; i++)
        {
            if (GMController.instance.decalDepot.depot[i].bulletType == type)
            {
                decal = GMController.instance.decalDepot.depot[i];
                break;
            }
        }
        decalList = new DecalInfo[poolSize];
        for (int i = 0; i < decalList.Length; i++)
        {
            GameObject bullet = Instantiate(decal.decal);
            decalList[i] = new DecalInfo(type, bullet);
        }
        poolReady = true;
    }
    private void Collector(int i)
    {
        decalList[i].isActive = false;
        decalList[i].decal.transform.parent = null;
        decalList[i].decal.transform.position = transform.position;
        if (decalList[i].enemyHit != null)
            decalList[i].enemyHit.decalsNum--;
        decalList[i].enemyHit = null;
        decalList[i].lifeTime = 0;
    }
}
