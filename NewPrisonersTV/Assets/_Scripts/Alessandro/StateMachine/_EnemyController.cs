using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine.AI;
using Character;

namespace AI
{
    public abstract class _EnemyController : MonoBehaviour
    {
        public Animator enemyAnim;
        public EnemyStats m_EnemyStats;
        public Transform[] raycastEyes;
        //------------------------------------------------------------------
        [BoxGroup("Only for Enemies with NavAgent")] public Transform[] patrolPoints;
        [BoxGroup("Only for Enemies with NavAgent")] public bool randomNavPoints;
        [BoxGroup("GroundCheck Sentinel")] public Transform checkPosition;
        [BoxGroup("Attack SpawnPoint")] public Transform attackSpawn;
        [BoxGroup("Kamikaze or Spider Explosion Particle")] public EnemyExplosionParticle explosionParticle;
        [BoxGroup("Mine Particle Only for Spider and Kamikaze")] public MineParticle mine; 
        [BoxGroup("Attack particle (if needed)")] public ParticleSystem attackParticle;
        //------------------------------------------------------------------
        [HideInInspector] public int currentDestinationCount = 0;
        [HideInInspector] public NavMeshAgent agent;
        [HideInInspector] public NavMeshPath path;
        [HideInInspector] public bool firstPatrolSet = false;
        [HideInInspector] public float currentPathTimer;
        //------------------------------------------------------------------
        [HideInInspector] public bool hasDecalsOn;
        [HideInInspector] public int decalsNum;
        //------------------------------------------------------------------
        [HideInInspector] public float animSpeed;
        //------------------------------------------------------------------
        [HideInInspector] public ENEMYTYPE enemyType;
        [HideInInspector] public int currentLife;
        //------------------------------------------------------------------  // COMPONENTS 
        [HideInInspector] public Rigidbody2D rb;
        [HideInInspector] public Collider2D col;
        [HideInInspector] public SpriteRenderer mySpriteRender;// temp
        [HideInInspector] public Transform thisTransform;
        [HideInInspector] public Transform thisMesh;
        [HideInInspector] public EnemySpawn myEnemySpawn;
        //------------------------------------------------------------------
        [HideInInspector] public int direction;                                // movement direction used for enemies without navmesh (1 = right, -1 = left)
        [HideInInspector] public int enemyOwnership;                           // number of player that last hit this enemy
        [HideInInspector] public bool isFlashing = false;
        [HideInInspector] public bool gotHit = false;
        //------------------------------------------------------------------
        [HideInInspector] public bool startDieCoroutine = false;                // starts the death coroutine if true
        [HideInInspector] public bool playerSeen = false;                       // true if a player is in sight
        [HideInInspector] public RaycastHit2D[] lineCastHits;
        [HideInInspector] public int playerSeenIndex;                           // index of player seen
        [HideInInspector] public int numRayHitPlayer;                           // used to check if all rays are not hitting the target
        [HideInInspector] public float currentViewTimer;                        // search for enemy every...
        [HideInInspector] public TargetDistance[] playerSeenDistance;           // records player index and distance from the agent
        //------------------------------------------------------------------
        #region BULLETS
        [HideInInspector] public float currentBulletTimer;
        [HideInInspector] public EnemyBulletParticle bullet;
        #endregion
        //------------------------------------------------------------------
        #region BATS
        [HideInInspector] public Vector3 startSwoopPosition;
        [HideInInspector] public Vector3 endSwoopPosition;
        [HideInInspector] public bool swoopCoroutineInExecution = false;
        [HideInInspector] public bool startSwoopCR = false;
        #endregion

        #region KAMIKAZE
        [HideInInspector] public float currentExplosionTimer;
        [HideInInspector] public bool canExplode;
        #endregion

        #region NINJA
        [HideInInspector] public Vector3 startJumpPos;
        [HideInInspector] public float currentJumpTimer;
        [HideInInspector] public bool onGround;
        #endregion

        #region DOG
        [HideInInspector] public float currentBiteTimer;
        [HideInInspector] public float currentDisengageTimer;
        [HideInInspector] public Vector3 lookAtPlayer;
        #endregion

        #region SENTINEL
        [HideInInspector] public int currentBarrageShots;
        [HideInInspector] public bool isBarrageDone = true;
        [HideInInspector] public bool canBarrageCR = false;
        #endregion
        //------------------------------------------------------------------ 

        protected virtual void Awake()
        {
            thisTransform = this.transform;
            rb = GetComponent<Rigidbody2D>(); 
            mySpriteRender = GetComponent<SpriteRenderer>();
            agent = GetComponent<NavMeshAgent>();
            col = thisTransform.GetChild(0).GetComponent<Collider2D>();
            thisMesh = enemyAnim.transform;
            enemyOwnership = -1;// check later
            animSpeed = enemyAnim.speed;
            currentLife = m_EnemyStats.life;
            playerSeenDistance = new TargetDistance[GMController.instance.playerInfo.Length];
            lineCastHits = new RaycastHit2D[1];
        }

        protected virtual void Start()
        {
            if (agent != null)
            {
                agent.speed = m_EnemyStats.speed;
                SetPatrolList();
            }
            currentViewTimer = m_EnemyStats.viewCheckFrequenzy;
        }
        //------------------------------------------------------------------
        protected virtual void Update()
        {
            if (GMController.instance.gameStart)
            {
                if (decalsNum <= 0)
                    hasDecalsOn = false;

                if (startDieCoroutine && !hasDecalsOn)
                {
                    StartCoroutine(Die());
                }
                if (!isFlashing && gotHit)
                {
                   // StartCoroutine(Flash());
                }               
            }
        }
        //------------------------------------------------------------------
        protected virtual void OnCollisionEnter2D(Collision2D collision){ }
        protected virtual void OnTriggerEnter2D(Collider2D collision){ }
        //------------------------------------------------------------------
        public void SetPatrolList()
        {
            if (patrolPoints.Length == 0)
            {
                if (enemyType == ENEMYTYPE.Dog)
                {
                    patrolPoints = new Transform[myEnemySpawn.DogPoints.Length];
                    patrolPoints = myEnemySpawn.DogPoints;
                }
                else if(enemyType == ENEMYTYPE.Kamikaze)
                {
                    patrolPoints = new Transform[myEnemySpawn.KamikazePoints.Length];
                    patrolPoints = myEnemySpawn.KamikazePoints;
                }
                else if(enemyType == ENEMYTYPE.Ninja)
                {
                    patrolPoints = new Transform[myEnemySpawn.NinjaPoints.Length];
                    patrolPoints = myEnemySpawn.NinjaPoints;
                }
                else if (enemyType == ENEMYTYPE.Spider)
                {
                    patrolPoints = new Transform[myEnemySpawn.SpiderPoints.Length];
                    patrolPoints = myEnemySpawn.SpiderPoints;
                }
            }
        }
        public void SetNextPatrolPoint()
        {
            if (patrolPoints.Length > 0)
            {
                if (randomNavPoints)
                    currentDestinationCount = Random.Range(0, patrolPoints.Length);
                else
                    currentDestinationCount++;

                // check if the destinationCount is within the patrolPoints lenght
                if (currentDestinationCount >= patrolPoints.Length)
                    currentDestinationCount = 0;
            }
            // set the new destination
            agent.destination = patrolPoints[currentDestinationCount].position;
            //Debug.Log(currentDestinationCount + "    " + agent.destination);  
        }
        public void DamagePlayer(Collider2D hit)
        {
            _CharacterController playerHit = hit.GetComponent<_CharacterController>();
            playerHit.currentLife -= m_EnemyStats.attackValue;
            if (playerHit.currentLife <= 0)
                playerHit.currentLife = 0;
            GMController.instance.UI.UpdateLifeUI(playerHit.playerNumber); // update life on UI
            GMController.instance.LowerTensionCheck(GMController.instance.tensionStats.playerHitPoints);// sub tension 
        }
        public void RotateTowardDirection(Transform transform, int direction)
        {
            if (direction < 0)
                transform.eulerAngles= new Vector3(0, 180, 0);
            else
                transform.eulerAngles = new Vector3(0, 0, 0);
        }
        public void MeshLookAtPlayerDir(int direction, float degrees)
        {
            if(direction < 0)
                thisMesh.localEulerAngles = new Vector3(0, -degrees, 0);
            else
                thisMesh.localEulerAngles = new Vector3(0, degrees, 0); 
        }
        public IEnumerator Barrage()
        {
            canBarrageCR = false;
            isBarrageDone = false;
            while (currentBarrageShots > 0 )
            {
                bullet.EmitBullet(attackSpawn, thisMesh.forward);
                currentBarrageShots--;
                yield return new WaitForSeconds(m_EnemyStats.barrageTimer);
            }
            currentBulletTimer = m_EnemyStats.bulletCooldown;
            currentBarrageShots = m_EnemyStats.numBarrageShots;
            isBarrageDone = true;
            yield return null;
        }
        public abstract IEnumerator Die();       
        //------------------------------------------------------------------




            //Flash coroutine called on hit with bullet
            //public IEnumerator Flash()
            //{
            //    isFlashing = true;
            //    mySpriteRender.color = Color.clear;
            //    yield return new WaitForSeconds(m_EnemyStats.flashingSpeed);
            //    mySpriteRender.color = Color.white;
            //    yield return new WaitForSeconds(m_EnemyStats.flashingSpeed);
            //    mySpriteRender.color = Color.clear;
            //    yield return new WaitForSeconds(m_EnemyStats.flashingSpeed);
            //    mySpriteRender.color = Color.white;
            //    yield return new WaitForSeconds(m_EnemyStats.flashingSpeed);
            //    mySpriteRender.color = Color.clear;
            //    yield return new WaitForSeconds(m_EnemyStats.flashingSpeed);
            //    mySpriteRender.color = Color.white;

            //    isFlashing = false;
            //    gotHit = false;
            //    yield return null; 
            //}     
    }
}

