using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Reward : MonoBehaviour
{
    [HideInInspector] public int poolIndex;

    public void Selection()
    {    
        GMController.instance.AddWeaponReward(GMController.instance.lastPlayerThatChooseReward, GMController.instance.bonusWeapon.bonusPool[poolIndex]);
        transform.parent = null;
        GMController.instance.UI.eventSystem.SetSelectedGameObject(GMController.instance.bonusWeapon.panel.GetChild(0).gameObject, new BaseEventData(GMController.instance.UI.eventSystem));
        Destroy(gameObject);
    }
}
