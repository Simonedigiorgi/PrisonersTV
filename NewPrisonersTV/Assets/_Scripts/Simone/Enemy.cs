using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int life;

    [Tooltip("Value off point earned by the player")]
    public int points;

    [HideInInspector]
    public bool isFlashing = false;

    public float flashingSpeed;

    private SpriteRenderer mySpriteRender;

    [Tooltip("Movement speed")]
    public int speed;

    [HideInInspector]
    public enum startDirectin { right, left }

    [HideInInspector ]
    public int enemyMembership;

    GameManager gameManager;

    bool startDieCoroutine = false;

    protected virtual void Start ()
    {
        //get the sprite rendere
        mySpriteRender = GetComponent<SpriteRenderer>();

        //Find game manager
        if(GameObject.Find("GameManager") != null)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        else
        {
            Debug.Log("ADD GAME MANAGER TO SCENE!!! (named 'GameManager')");
        }
    }

    protected virtual void Update()
    {
        //enemy die
        if (life <= 0 && !startDieCoroutine)
        {
            startDieCoroutine = true;
            StartCoroutine(Die());
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

    //Flash coroutine called on hit with bullet
    public IEnumerator Flash()
    {
        isFlashing = true;
        mySpriteRender.color = Color.clear;
        yield return new WaitForSeconds(flashingSpeed);
        mySpriteRender.color = Color.white;
        yield return new WaitForSeconds(flashingSpeed);
        mySpriteRender.color = Color.clear;
        yield return new WaitForSeconds(flashingSpeed);
        mySpriteRender.color = Color.white;
        yield return new WaitForSeconds(flashingSpeed);
        mySpriteRender.color = Color.clear;
        yield return new WaitForSeconds(flashingSpeed);
        mySpriteRender.color = Color.white;

        isFlashing = false;
    }

    // this coroutine was created to give the time at membership to change and for make shure the score is assigned right
    public IEnumerator Die()
    {
        yield return new WaitForEndOfFrame();

        //add score
        if (enemyMembership == 1)
        {
            gameManager.P1Score += points;
        }
        else if (enemyMembership == 2)
        {
            gameManager.P2Score += points;
        }

        Destroy(gameObject);
    }
}
