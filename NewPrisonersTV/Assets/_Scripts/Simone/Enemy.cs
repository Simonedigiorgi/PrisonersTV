using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int life;

    [HideInInspector] public bool isFlashing = false;

    public float flashingSpeed;

    private SpriteRenderer mySpriteRender;

    [Tooltip("Movement speed")] public int speed;

    [HideInInspector] public enum startDirectin { right, left }

    protected virtual void Start ()
    {
        //get the sprite rendere
        mySpriteRender = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        //enemy die
        if (life <= 0)
        {
            Destroy(gameObject);
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
}
