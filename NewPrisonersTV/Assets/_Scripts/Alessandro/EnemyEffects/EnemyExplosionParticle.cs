using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;
using Character;

public class EnemyExplosionParticle : MonoBehaviour
{
    public _EnemyController owner;
    public ParticleSystem thisParticle;
    public ParticleSystem mine;

    private ParticleSystem.EmissionModule emission;
    private LayerMask explosionMask;
    private int damage;
    private int min;
    private int max;

    private void Start()
    {
        // copy components
        explosionMask = owner.m_EnemyStats.hitMask;
        damage = owner.m_EnemyStats.attackValue;
        // get the particle burst info
        emission = thisParticle.emission;
        ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[emission.burstCount];
        emission.GetBursts(bursts);
        min = bursts[0].minCount;
        max = bursts[0].maxCount;
        transform.parent = null;
    }

    public void Explosion(Vector2 position)
    {       
       // emit particle       
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
            GMController.instance.LowerTensionCheck(GMController.instance.tensionStats.playerHitPoints);// sub tension
        }
    }

    private void Update()  
    {
        // destroy when the last explosion fades and the owner is dead
        if (owner == null && mine == null && thisParticle.particleCount == 0)  
            Destroy(gameObject);
    }
}
