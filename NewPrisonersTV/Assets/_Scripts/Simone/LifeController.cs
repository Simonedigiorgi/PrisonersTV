using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class LifeController : MonoBehaviour {

    private PlayerController player;                                                                // Get PlayerController script
    private Rigidbody2D rb;                                                                         // Rigidbody components

    [Range(0, 3)]
    [BoxGroup("Controls")] public int life;                                                         // Player life
    [BoxGroup("Controls")] public float respawnTime;                                                // Time before respawning

    private void Awake()
    {
        player = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        life = 3;
    }

    void Update () {

        if (player.isActive)
        {
            // Player life
            if (life <= 0)
            {
                life = 0;
                StartCoroutine(Death());
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // When player trigger an enemy
        if (collision.gameObject.CompareTag("Enemy"))
            life--;
    }

    public IEnumerator Death()
    {
        // Set the Arm_Anim sprite to false
        player.playerArm.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;

        // Death animation
        player.playerAnim.SetBool("Death", true);

        // Stop the player and set active to false
        rb.velocity = new Vector2(0, 0);
        player.isActive = false;

        // Disable object after
        yield return new WaitForSeconds(respawnTime);

        // Destroy the weapon
        if (player.playerArm.transform.GetChild(0).childCount > 0)
        {
            GameObject first = player.playerArm.transform.GetChild(0).transform.GetChild(0).gameObject;
            Destroy(first.gameObject);
        }

        // Disable the Player
        gameObject.SetActive(false);
    }
}
