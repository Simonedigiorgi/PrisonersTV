using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Prototype/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    #region INSPECTOR BOOLEANS
    private bool kamiSpiNinDog = false;
    private bool ninSent = false;
    private bool dogSpiKami = false;
    private bool dogSentSpi = false;
    private bool kamiAndSpider = false;
    #endregion

    [OnValueChanged("HideStuff")]
    public ENEMYTYPE type;
    public int life;
    [Range(1, 3)]
    [Tooltip("Enemy start level")] public int enemyLevel = 1;
    [Tooltip("points earned by the player")] public int points;
    [Tooltip("View distance")] public float attackView;
    [ShowIf("dogSentSpi", true)] [Tooltip("View distance while attacking target")] public float chasingView;
    [Tooltip("view check every...")] public float viewCheckFrequenzy;
    [ShowIf("dogSpiKami", true)] [Tooltip("check path every...")] public float pathCheckFrequenzy; 
    [Tooltip("view obstacle")] public LayerMask obstacleMask;
    [Tooltip("Movement speed")] public int speed;
    [ShowIf("dogSpiKami", true)] [Tooltip("Chasing speed")] public float runSpeed;
    [ShowIf("kamiSpiNinDog", true)] [Tooltip("NavLink Speed")] public float jumpSpeed;
    [Tooltip("Damage done to target")] public int attackValue; 
    [Tooltip("Weakness")] public ResistanceAndWeakness[] weakness;
    [Tooltip("Resistance")] public ResistanceAndWeakness[] resistance; 
    //---------------------------------------------------------------------------------------
    public LayerMask hitMask;
    [ShowIf("ninSent", true)] public int bulletNumberOfHits;
    [ShowIf("ninSent", true)] public bool canBulletBounce;
    [ShowIf("ninSent", true)] public float bulletCooldown;
    [ShowIf("ninSent", true)] public float bulletLifeTime;
    [ShowIf("ninSent", true)] public float bulletSpeed;
    [ShowIf("ninSent", true)] public float bulletGravity;
    [ShowIf("ninSent", true)] public float bulletRayLenght;  
    //---------------------------------------------------------------------------------------
    #region BATS
    [ShowIf("type", ENEMYTYPE.Bat)][Tooltip("Time needed for the swoop")] [Range(0.5f, 3)]
    public float swoopMoreSlowly;
    [ShowIf("type", ENEMYTYPE.Bat)][Tooltip("Initial movement directions")] 
    public STARTDIRECTION myStartDirection;
    [ShowIf("type", ENEMYTYPE.Bat)][Tooltip("Amplitude of sinusoidal movement")] 
    public int sinusoidalMovement;
    #endregion 
    //---------------------------------------------------------------------------------------
    #region NINJA
    [ShowIf("type", ENEMYTYPE.Ninja)]
    public float ninjaJumpLenght;
    [ShowIf("type", ENEMYTYPE.Ninja)]
    public float ninjaJumpHeight;
    [ShowIf("type", ENEMYTYPE.Ninja)]
    public float ninjaJumpCooldown;
    [ShowIf("type", ENEMYTYPE.Ninja)]
    public float groundCheckRadius;
    #endregion
    //---------------------------------------------------------------------------------------
    #region KAMIKAZE
    [ShowIf("type", ENEMYTYPE.Kamikaze)]
    public float explosionTimer;
    #endregion
    //---------------------------------------------------------------------------------------
    #region KAMIKAZE & SPIDER
    [ShowIf("kamiAndSpider", true)]
    public float bombsTimer; 
    [ShowIf("kamiAndSpider", true)]
    public float bombsXseconds;
    #endregion
    //---------------------------------------------------------------------------------------
    #region SPIDER
    [ShowIf("type", ENEMYTYPE.Spider)]
    public int bombsOnDeath;
    #endregion
    //---------------------------------------------------------------------------------------
    #region DOG
    [ShowIf("type", ENEMYTYPE.Dog)]
    public float triggerBiteDistance;
    [ShowIf("type", ENEMYTYPE.Dog)]
    public float biteRadius;
    [ShowIf("type", ENEMYTYPE.Dog)]
    public float biteCooldown;
    [ShowIf("type", ENEMYTYPE.Dog)]
    public float disengageTimer;
    #endregion
    //---------------------------------------------------------------------------------------
    #region SENTINEL
    [ShowIf("type", ENEMYTYPE.Sentinel)]
    public float groundRayDistance;
    [ShowIf("type", ENEMYTYPE.Sentinel)]
    public int numBarrageShots;
    [ShowIf("type", ENEMYTYPE.Sentinel)]
    public float barrageTimer;
    #endregion

    
    private void HideStuff()
    {
        kamiAndSpider = (type == ENEMYTYPE.Kamikaze || type == ENEMYTYPE.Spider); 
        dogSentSpi = (type == ENEMYTYPE.Dog || type == ENEMYTYPE.Sentinel || type == ENEMYTYPE.Spider);
        dogSpiKami = (type == ENEMYTYPE.Kamikaze || type == ENEMYTYPE.Dog || type == ENEMYTYPE.Spider);
        ninSent = (type == ENEMYTYPE.Ninja || type == ENEMYTYPE.Sentinel);
        kamiSpiNinDog = (type == ENEMYTYPE.Kamikaze || type == ENEMYTYPE.Dog || type == ENEMYTYPE.Spider || type == ENEMYTYPE.Ninja);
    }
}
