using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class MineParticle : MonoBehaviour
{
    public _EnemyController owner;
    public ParticleSystem thisParticle;

    [HideInInspector] public bool isActive = false;

    private ParticleSystem.Particle[] mines;

    private void Update()
    {
        if(isActive)
        {
            // enable/disable the emission module
            if (!thisParticle.emission.enabled && owner != null)
            {
                thisParticle.transform.parent = null;
                ParticleSystem.EmissionModule particle = thisParticle.emission;
                particle.enabled = true;
            }
            else if(owner == null && thisParticle.emission.enabled)
            {
                ParticleSystem.EmissionModule particle = thisParticle.emission;
                particle.enabled = false;
            }

            // relocate the particle system on the owner
            if (owner != null)
                thisParticle.transform.position = owner.thisTransform.position;
            
            // trackdown all the alive particles
            if (mines == null || mines.Length < thisParticle.main.maxParticles)
                mines = new ParticleSystem.Particle[thisParticle.main.maxParticles];

            int numParticlesAlive = thisParticle.GetParticles(mines);
            // let the dying particles explode
            for (int i = 0; i < numParticlesAlive; i++)
            {    
                bool alreadyExploded = false;
                if (mines[i].remainingLifetime <= 0.1 && !alreadyExploded)
                {
                    owner.explosionParticle.Explosion(mines[i].position);
                    alreadyExploded = true;
                    mines[i].remainingLifetime = 0;
                }
            }
            // Apply the particle changes to the particle system
            thisParticle.SetParticles(mines, numParticlesAlive);

            // destroy when the explosion fades
            if (owner == null && thisParticle.particleCount == 0)
                Destroy(gameObject);
        }
    }

}
