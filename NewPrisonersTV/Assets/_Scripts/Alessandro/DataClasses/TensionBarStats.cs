using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Prototype/TensionBarStats")]
public class TensionBarStats : ScriptableObject
{
    public int maxBarLevel;
    public int standardBarCapacity;
    public float multiXPlayer;
    public int barDivision = 1;  

    [BoxGroup("Increments")] [Tooltip("timer for movementPoints")]
    public float movementTimer;
    [BoxGroup("Increments")] [Tooltip("points given when moving")]
    public int movementPoints;
    [BoxGroup("Increments")] [Tooltip("points given when jumping, rolling and such")]
    public int actionsPoints;
    [BoxGroup("Increments")] [Tooltip("points given when hitting enemies")]
    public int enemyHitPoints;
    [BoxGroup("Increments")] [Tooltip("points given when killing enemies")]
    public int enemyKillPoints;

    [BoxGroup("Decrements")] [Tooltip("timer for standStillPoints")]
    public int standStillTimer;
    [BoxGroup("Decrements")] [Tooltip("points taken when NOT moving")]
    public int standStillPoints;
    [BoxGroup("Decrements")] [Tooltip("points taken when player get hit")]
    public int playerHitPoints;
    [BoxGroup("Decrements")] [Tooltip("points taken when player dies")]
    public int playerDeathPoints;

    [BoxGroup("Threshold Bonuses")]
    [TableMatrix(DrawElementMethod = "DrawBonusClass", ResizableColumns = false, RowHeight = 30)]
    public TensionBonus[] bonus;
}

