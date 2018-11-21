using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine.AI;
namespace AI
{
    public abstract class _EnemyController : MonoBehaviour
    {
        public EnemyStats m_EnemyStats;
        public Transform[] raycastEyes;

        [BoxGroup("Only for Enemies with NavAgent")] public Transform[] patrolPoints;
        [BoxGroup("Only for Enemies with NavAgent")] public bool randomNavPoints;

        [BoxGroup("Kamikaze Explosion Particle")] public KamikazeExplosionParticle explosionParticle;
        [BoxGroup("Kamikaze Explosion Particle")] public LayerMask explosionMask;

        [BoxGroup("Mine Particle Only for Spider and Kamikaze")] public MineParticle mine;
        [BoxGroup("Mine Particle Only for Spider and Kamikaze")] public LayerMask mineMask;

        [HideInInspector] public int currentDestinationCount = 0;
        [HideInInspector] public NavMeshAgent agent;
        
        [HideInInspector] public bool hasDecalsOn;
        [HideInInspector] public int decalsNum;

        [HideInInspector] public Animator enemyAnim;
        [HideInInspector] public float animSpeed;

        [HideInInspector] public ENEMYTYPE enemyType;
        [HideInInspector] public int currentLife;

        [HideInInspector] public Rigidbody2D rb;
        [HideInInspector] public SpriteRenderer mySpriteRender;
        [HideInInspector] public Transform thisTransform;

        [HideInInspector] public int direction;
        [HideInInspector] public int enemyMembership;
        [HideInInspector] public bool isFlashing = false;
        [HideInInspector] public bool gotHit = false;

        [HideInInspector] public bool startDieCoroutine = false;
        [HideInInspector] public bool playerSeen = false;
        [HideInInspector] public int playerSeenIndex;

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

        protected virtual void Awake()
        {
            thisTransform = this.transform;
            mySpriteRender = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
            currentLife = m_EnemyStats.life;
            enemyAnim = GetComponent<Animator>();
            //animSpeed = enemyAnim.speed;
            agent = GetComponent<NavMeshAgent>();         
        }
        
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

        protected virtual void OnCollisionEnter2D(Collision2D collision){ }
        protected virtual void OnTriggerEnter2D(Collider2D collision){ }

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
        }

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

        // this coroutine was created to give the time at membership to change and for make shure the score is assigned right
        public abstract IEnumerator Die();
      
    }
}

