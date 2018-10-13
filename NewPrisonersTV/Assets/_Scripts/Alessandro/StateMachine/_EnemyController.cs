using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace AI
{
    public abstract class _EnemyController : MonoBehaviour
    {
        public EnemyStats m_EnemyStats;
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
        //Bats only
        [HideInInspector] public Vector3 startSwoopPosition;
        [HideInInspector] public Vector3 endSwoopPosition;
        [HideInInspector] public bool swoopCoroutineInExecution = false;
        [HideInInspector] public bool startSwoopCR = false;

        protected virtual void Awake()
        {
            //get the sprite rendere
            thisTransform = this.transform;
            mySpriteRender = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
            currentLife = m_EnemyStats.life;
            enemyAnim = GetComponent<Animator>();
            animSpeed = enemyAnim.speed;
            //assign start directions
            if (m_EnemyStats.myStartDirection == STARTDIRECTION.Right)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }
        }

        protected virtual void Update()
        {
            if (GMController.instance.gameStart)
            {
                if (startDieCoroutine)
                {
                    StartCoroutine(Die());
                }
                if (!isFlashing && gotHit)
                {
                    StartCoroutine(Flash());
                }               
            }
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            //turn the direction if collide on wall
            if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Enemy"))
            {
                direction *= -1; 
            }
        }    

        //Flash coroutine called on hit with bullet
        public IEnumerator Flash()
        {
            isFlashing = true;
            mySpriteRender.color = Color.clear;
            yield return new WaitForSeconds(m_EnemyStats.flashingSpeed);
            mySpriteRender.color = Color.white;
            yield return new WaitForSeconds(m_EnemyStats.flashingSpeed);
            mySpriteRender.color = Color.clear;
            yield return new WaitForSeconds(m_EnemyStats.flashingSpeed);
            mySpriteRender.color = Color.white;
            yield return new WaitForSeconds(m_EnemyStats.flashingSpeed);
            mySpriteRender.color = Color.clear;
            yield return new WaitForSeconds(m_EnemyStats.flashingSpeed);
            mySpriteRender.color = Color.white;

            isFlashing = false;
            gotHit = false;
            yield return null; 
        }

        // this coroutine was created to give the time at membership to change and for make shure the score is assigned right
        public abstract IEnumerator Die();
      
    }
}

