using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class UnicornSpecial : PerforationBullet
{
    public float sinusoidalAmp;
    protected override void Update()
    {
        if (GMController.instance.gameStart && magazineReady)
        {
            if (GMController.instance.playerInfo[playerNumber].playerController.currentWeapon != null && 
                GMController.instance.playerInfo[playerNumber].playerController.currentWeapon.perfBullet == type &&
                playerWeapon == null)
            {
                playerWeapon = GMController.instance.playerInfo[playerNumber].playerController.currentWeapon;
            }

            if(playerWeapon != null)
              CopyStats();

            for (int i = 0; i < perfPool.Length; i++)
            {
                if (perfPool[i].isActive)
                {
                    // Movement
                    perfPool[i].velocity = (perfPool[i].dir * bulletSpeed);
                    perfPool[i].bullet.transform.Translate(perfPool[i].velocity* Time.deltaTime, Space.World);
    
                    Vector3 _newPosition = perfPool[i].bullet.transform.GetChild(0).localPosition;
                    _newPosition.y += Mathf.Sin(Time.time*sinusoidalAmp) * Time.deltaTime * sinusoidalAmp;
                    perfPool[i].bullet.transform.GetChild(0).localPosition = _newPosition;

                    // timer CD
                    perfPool[i].lifeTime -= Time.deltaTime; 

                    // Raycast check for Collision 
                    EnvoiromentRaycastCheck(i);

                    if (perfPool[i].lifeTime <= 0)                    
                        Collector(i);
                }
            }           
        }
    }
    public override void ShootPerf(Transform spawnPoint)
    {

        for (int i = 0; i < perfPool.Length; i++)
        {
            if (!perfPool[i].isActive)
            {
                perfPool[i].bullet.transform.position = spawnPoint.position;
                perfPool[i].bullet.transform.rotation = Quaternion.LookRotation(spawnPoint.forward, spawnPoint.up);
                perfPool[i].lifeTime = bulletLifeTime;
                perfPool[i].numberOfHits = playerWeapon.numberOfHits;
                perfPool[i].enemyHit = new _EnemyController[perfPool[i].numberOfHits];
                perfPool[i].bullet.transform.GetChild(0).gameObject.SetActive(true);
                perfPool[i].isActive = true;
                break;
            }
        }
    }
    protected override void Collector(int i)
    {
        perfPool[i].bullet.transform.GetChild(0).gameObject.SetActive(false);
        base.Collector(i);
        perfPool[i].bullet.transform.GetChild(0).localPosition = Vector3.zero;
    }
}
