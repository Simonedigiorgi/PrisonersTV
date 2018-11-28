using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;
using Character;

public class KamikazeExplosionParticle : MonoBehaviour
{
    public _EnemyController owner;
    public ParticleSystem thisParticle;
    public ParticleSystem mine;

    private LayerMask explosionMask;
    private int damage;

    private void Start()
    {
        // copy components
        explosionMask = owner.m_EnemyStats.explosionMask;
        damage = owner.m_EnemyStats.attackValue;
    }

    public void Explosion(Vector2 position)
    {
        // get the particle burst info
        var emission = thisParticle.emission;
        ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[emission.burstCount];
        emission.GetBursts(bursts);
        int min = bursts[0].minCount;
        int max = bursts[0].maxCount;

        // emit particle
        transform.parent = null;
        transform.position = position;
        thisParticle.Emit(Random.Range(min, max));
       
        // check collision with players
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, thisParticle.shape.radius, explosionMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            _CharacterController playerHit = colliders[i].GetComponent<_CharacterController>();
            playerHit.currentLife -= damage;
            if (playerHit.currentLife <= 0) 
                playerHit.currentLife = 0;
            GMController.instance.UI.UpdateLifeUI(playerHit.playerNumber); // update life on UI
        }
    }

    private void Update()
    {
        // destroy when the explosion fades
        if (owner == null && mine == null && thisParticle.particleCount == 0)
            Destroy(gameObject);
    }
}
