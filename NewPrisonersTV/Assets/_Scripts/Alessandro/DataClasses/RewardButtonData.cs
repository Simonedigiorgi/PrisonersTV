using UnityEngine;
using UnityEngine.UI;
public class RewardButtonData
{
    private int poolIndex;
    private GameObject rewardButton;
    private Transform buttonT;

    public int PoolIndex { get { return poolIndex; } }
    public GameObject RewardButton { get { return rewardButton; } }
    public Transform ButtonT { get { return buttonT; } }

    public RewardButtonData(int index, GameObject button, Sprite icon )
    {
        poolIndex = index;
        rewardButton = button;
        buttonT = button.transform; 
        rewardButton.GetComponent<Image>().sprite = icon;
        rewardButton.SetActive(true);
    }
}
