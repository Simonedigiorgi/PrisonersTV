using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BonusWeapon : MonoBehaviour
{
    public WeaponList weaponList;
    public int poolSize;
    public int midGrade;
    public int specialGrade;
    public Transform panel;
    public Button button;

    [HideInInspector] public Weapon3D[] bonusPool;

    void Awake ()
    {
        bonusPool = new Weapon3D[poolSize];
	}
	
	public void RewardPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            Button currentButton; 

            if(i < midGrade)
            {
                bonusPool[i] = weaponList.MidGrade[Random.Range(0, weaponList.MidGrade.Count)];   
            }
            else if(i > midGrade && i < specialGrade)
            {
                bonusPool[i] = weaponList.Special[Random.Range(0, weaponList.Special.Count)];
            }
            else
            {
                int luck = Random.Range(0,3);
                if(luck == 0)
                    bonusPool[i] = weaponList.LowGrade[Random.Range(0, weaponList.LowGrade.Count)];
                else if(luck == 1)
                    bonusPool[i] = weaponList.MidGrade[Random.Range(0, weaponList.MidGrade.Count)];
                else if(luck == 2)
                    bonusPool[i] = weaponList.Special[Random.Range(0, weaponList.Special.Count)];
            }
            
            currentButton = Instantiate(button, panel);
            currentButton.gameObject.SetActive(true);
            button.image = bonusPool[i].weaponIcon;      
            currentButton.GetComponent<Reward>().poolIndex = i;
        }
        GMController.instance.UI.eventSystem.SetSelectedGameObject(panel.GetChild(0).gameObject, new BaseEventData(GMController.instance.UI.eventSystem));
        GMController.instance.canChooseReward = true;
    }

   
}
