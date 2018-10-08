using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace AI
{
    public class _EnemyController : MonoBehaviour
    {
        public EnemyStats m_EnemyStats;

        [HideInInspector] public Rigidbody2D rb;
        [HideInInspector] public SpriteRenderer mySpriteRender;
        [HideInInspector] public Transform thisTransform;

        [HideInInspector] public int direction;
        [HideInInspector] public int enemyMembership;
        [HideInInspector] public bool isFlashing = false;

        [HideInInspector] public Vector3 startSwoopPosition;
        [HideInInspector] public Vector3 endSwoopPosition;

        [HideInInspector] public bool startDieCoroutine = false;
        [HideInInspector] public bool playerSeen = false;

        [HideInInspector] public bool swoopCoroutineInExecution = false;
        [HideInInspector] public bool startSwoopCR = false;

        private void Awake()
        {
            //get the sprite rendere
            thisTransform = this.transform;
            mySpriteRender = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();

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

        private void Update()
        {
            if(startDieCoroutine)
            {
                StartCoroutine(Die());
            }

            if (startSwoopCR)
            {
                StartCoroutine(Swoop());
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

        public void OnTriggerEnter2D(Collider2D collision)
        {
            //on collision with bullet
            if (collision.gameObject.CompareTag("Bullet"))
            {
                //start flashing feedback
                if (!isFlashing)
                {
                    StartCoroutine(Flash());
                }
            }
        }

        public IEnumerator Swoop()
        {
            startSwoopCR = false;
            swoopCoroutineInExecution = true;
            transform.DOMove(endSwoopPosition, m_EnemyStats.swoopMoreSlowly, false);
            yield return new WaitUntil(() => transform.position == endSwoopPosition);

            transform.DOMove(startSwoopPosition, m_EnemyStats.swoopMoreSlowly, false);
            yield return new WaitUntil(() => transform.position == startSwoopPosition);

            playerSeen = false;
            swoopCoroutineInExecution = false;
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
        }

        // this coroutine was created to give the time at membership to change and for make shure the score is assigned right
        public IEnumerator Die()
        {
            yield return new WaitForEndOfFrame();

            GMController.instance.playerInfo[enemyMembership].score += m_EnemyStats.points;           

            Destroy(gameObject);
        }
    }
}

