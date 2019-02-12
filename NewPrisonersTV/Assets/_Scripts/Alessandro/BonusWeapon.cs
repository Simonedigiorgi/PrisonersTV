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
    public Sprite unknownIcon;

    [HideInInspector] public Weapon3D[] bonusPool;
    [HideInInspector] public RewardButtonData[] rewardButtons;
    
    void Awake ()
    {
        bonusPool = new Weapon3D[poolSize];
        rewardButtons = new RewardButtonData[poolSize];
    }
	
	public void RewardPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            if(i <= midGrade)
            {
                bonusPool[i] = weaponList.MidGrade[Random.Range(0, weaponList.MidGrade.Count)];
                ButtonCreation(i, bonusPool[i].weaponIcon.sprite);
            }
            else if(i > midGrade && i < specialGrade)
            {
                bonusPool[i] = weaponList.Special[Random.Range(0, weaponList.Special.Count)];
                ButtonCreation(i, bonusPool[i].weaponIcon.sprite);
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

                ButtonCreation(i, unknownIcon);
            }
        }
        GMController.instance.eventSystem.SetSelectedGameObject(panel.GetChild(0).gameObject, new BaseEventData(GMController.instance.eventSystem)); 
        GMController.instance.canChooseReward = true;
    }

    void ButtonCreation( int i, Sprite icon)
    {
        Button currentButton = Instantiate(button, panel);

        rewardButtons[i] = new RewardButtonData(i, currentButton.gameObject, icon);
    }  
}
