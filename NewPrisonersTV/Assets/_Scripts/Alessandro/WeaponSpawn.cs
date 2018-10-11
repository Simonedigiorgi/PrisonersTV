using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawnRate
{
    public float rate;
    public List<Weapon3D> weaponList;

    public WeaponSpawnRate(float r, List<Weapon3D> list)
    {
        rate = r;
        weaponList = list;
    }
}

public class WeaponSpawn : MonoBehaviour
{
    public float spawnTimer;
    public bool totalRandom;
    [Range(0, 1)] public float lowGradeRate;
    [Range(0, 1)] public float midGradeRate;
    [Range(0, 1)] public float specialGradeRate;
    public WeaponList weaponList;

    private Weapon3D currentWeapon;
    private float timer;
    private Animator anim;
    private bool spawnDone = true;

    private WeaponSpawnRate biggerRate;
    private WeaponSpawnRate mediumRate;
    private WeaponSpawnRate lowerRate;

    void Start()
    {
        anim = GetComponent<Animator>();
        ResetTimer();

        // fix slider proportions 
        float sum = lowGradeRate + midGradeRate + specialGradeRate;
        if (sum > 1)
        {
            float difference = sum - 1;
            lowGradeRate -= difference / 3;
            midGradeRate -= difference / 3;
            specialGradeRate -= difference / 3;
        }
        else if(sum < 1)
        {
            float difference = 1 - sum;
            lowGradeRate += difference / 3;
            midGradeRate += difference / 3;
            specialGradeRate += difference / 3;
        }
        // find bigger, medium and lower rates between the sliders
        float bigger = Mathf.Max(lowGradeRate,midGradeRate,specialGradeRate);

        if (bigger == lowGradeRate)
        {
            biggerRate = new WeaponSpawnRate(lowGradeRate, weaponList.LowGrade);
            if (Mathf.Max(midGradeRate, specialGradeRate) == midGradeRate || midGradeRate == specialGradeRate)
            {
                mediumRate = new WeaponSpawnRate(midGradeRate, weaponList.MidGrade);
                lowerRate = new WeaponSpawnRate(specialGradeRate, weaponList.Special);
            }
            else
            {
                mediumRate = new WeaponSpawnRate(specialGradeRate, weaponList.Special);
                lowerRate = new WeaponSpawnRate(midGradeRate, weaponList.MidGrade);
            }           
        }
        else if (bigger == midGradeRate)
        {
            biggerRate = new WeaponSpawnRate(midGradeRate, weaponList.MidGrade);
            if (Mathf.Max(lowGradeRate, specialGradeRate) == lowGradeRate || lowGradeRate == specialGradeRate)
            {
                mediumRate = new WeaponSpawnRate(lowGradeRate, weaponList.LowGrade);
                lowerRate = new WeaponSpawnRate(specialGradeRate, weaponList.Special);
            }
            else
            {
                mediumRate = new WeaponSpawnRate(specialGradeRate, weaponList.Special);
                lowerRate = new WeaponSpawnRate(lowGradeRate, weaponList.LowGrade);
            }
        }
        else
        {
            biggerRate = new WeaponSpawnRate(specialGradeRate, weaponList.Special);
            if (Mathf.Max(lowGradeRate, midGradeRate) == lowGradeRate || lowGradeRate == midGradeRate)
            {
                mediumRate = new WeaponSpawnRate(lowGradeRate, weaponList.LowGrade);
                lowerRate = new WeaponSpawnRate(midGradeRate, weaponList.MidGrade);
            }
            else
            {
                mediumRate = new WeaponSpawnRate(midGradeRate, weaponList.MidGrade);
                lowerRate = new WeaponSpawnRate(lowGradeRate, weaponList.LowGrade);
            }
        }
    }


    void Update()
    {
        if (GMController.instance.gameStart)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }        

            if (timer <= 0 && spawnDone && currentWeapon == null)
            {
                if (totalRandom)
                {
                    int i = Random.Range(0, 3);

                    if (i == 0 && weaponList.LowGrade.Count > 0)
                    {
                        StartCoroutine(SpawnWeaponFromList(weaponList.LowGrade));
                    }
                    else if (i == 1 && weaponList.MidGrade.Count > 0)
                    {
                        StartCoroutine(SpawnWeaponFromList(weaponList.MidGrade));
                    }
                    else if (i == 2 && weaponList.Special.Count > 0)
                    {
                        StartCoroutine(SpawnWeaponFromList(weaponList.Special));
                    }
                }
                else
                {
                    float i = Random.value;

                    if (i <= biggerRate.rate) // bigger rate weapon grade
                    {
                        StartCoroutine(SpawnWeaponFromList(biggerRate.weaponList));
                    }
                    if (i <= (biggerRate.rate + mediumRate.rate) && i > biggerRate.rate) // medium rate weapon grade
                    {
                        StartCoroutine(SpawnWeaponFromList(mediumRate.weaponList));
                    }
                    else if (i > (biggerRate.rate + mediumRate.rate)) // lower rate weapon grade
                    {
                        StartCoroutine(SpawnWeaponFromList(lowerRate.weaponList));
                    }


                }
            }
        }
    }

    private IEnumerator SpawnWeaponFromList(List<Weapon3D> weapons)
    {
        spawnDone = false;
        anim.SetInteger("State", 1);
        yield return new WaitForSeconds(0.2f);

        GameObject newWeapon = Instantiate(weapons[Random.Range(0, weapons.Count)].gameObject, transform.position, Quaternion.identity);
        currentWeapon = newWeapon.GetComponent<Weapon3D>();
        currentWeapon.currentSpawn = this;
        anim.SetInteger("State", 2);

        yield return new WaitForSeconds(0.2f);
        anim.SetInteger("State", 0);        
        spawnDone = true;
        yield return null;
    }

    public Weapon3D GetCurrentWeapon()
    {
        return currentWeapon;
    }
    public void SetCurrentWeaponToNull()
    {
        currentWeapon = null;
    }

    public void ResetTimer()
    {
        timer = spawnTimer;
    }
}
