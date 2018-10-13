using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public abstract class ParticleEmitterRaycastBullet : MonoBehaviour
{
    public ParticleSystem Gun;
    public int damage;
    public float rayLenght = 1;
    public float bulletGravity;
    public bool canBounce;

    [HideInInspector] public ParticleSystem afterBurn;
    [HideInInspector] public ParticleSystem.Particle[] bullets;
    [HideInInspector] public int membership;

    //public Gradient bulletColor; 
    //public float colorRange;
    public abstract void EmitBullet();
 
    protected void EmitAtLocation(Vector3 collision, Vector3 collisionNormal)
    {                       
        ParticleSystem.MainModule psMain = afterBurn.main;
        //psMain.startColor = bulletColor.Evaluate(colorRange);
        afterBurn.transform.position = collision;
        afterBurn.transform.rotation = Quaternion.LookRotation(collisionNormal);

        afterBurn.Emit(1);
    }
    

    protected virtual void Update ()
    {
        if (bullets == null || bullets.Length < Gun.main.maxParticles)
            bullets = new ParticleSystem.Particle[Gun.main.maxParticles];

        int numParticlesAlive = Gun.GetParticles(bullets);

        for (int i = 0; i < numParticlesAlive; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(bullets[i].position, bullets[i].velocity.normalized, rayLenght);
            Debug.DrawRay(bullets[i].position, bullets[i].velocity.normalized, Color.blue); 
            if (hit) 
            {              
                if (hit.transform.CompareTag("Enemy"))
                {
                    bullets[i].startLifetime = 0;

                    _EnemyController enemyHit = hit.transform.GetComponent<_EnemyController>();
                    enemyHit.enemyMembership = membership;
                    enemyHit.currentLife -= damage;
                    enemyHit.gotHit = true; 
                }  
                else
                {
                    bullets[i].startLifetime = 0;
                }
            }

        }
        // Apply the particle changes to the particle system
        Gun.SetParticles(bullets, numParticlesAlive);
	}
}
