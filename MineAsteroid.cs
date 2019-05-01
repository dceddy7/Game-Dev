using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Author: Jacob Eddy
 * Created: 4/30
 * Last modified 5/1/2018 
 */
public class MineAsteroid : MonoBehaviour {

    public int damagePerShot = 5;                  // The damage inflicted by each bullet.
    public float timeBetweenBullets = 0.1f;        // The time between each shot.
    public float range = 100f;                      // The distance the gun can fire.
    public int efficiencyVal = 1;
    public float timer;                                    // A timer to determine when to fire.
    public Ray shootRay;                                   // A ray from the gun end forwards.
    public RaycastHit shootHit;                            // A raycast hit to get information about what was hit.
    public int shootableMask;                              // A layer mask so the raycast only hits things on the shootable layer.
    public ParticleSystem gunParticles;                    // Reference to the particle system.
    public LineRenderer gunLine;                           // Reference to the line renderer.
    private Ship thePlayer;
    private int effectiveEfficiencyVal = 0;
    // public AudioSource gunAudio;                           // Reference to the audio source.
  //  public Light gunLight;                                 // Reference to the light component.
    public float effectsDisplayTime = 1f;                // The proportion of the timeBetweenBullets that the effects will display for.



    void Awake()
    {
        // Create a layer mask for the Shootable layer.
        shootableMask = LayerMask.GetMask("Shootable");
        thePlayer = GameObject.Find("PlayerShipOuterShell").GetComponent<Ship>();

    }

    void Update()
    {
        timer += Time.deltaTime;

        effectiveEfficiencyVal = (int)Mathf.Ceil(efficiencyVal * thePlayer.miningEfficiency);

    }

    public void DisableEffects()
    {
        // Disable the line renderer and the light.
        gunLine.enabled = false;
   
    }

    public void Shoot()
    {
        // Reset the timer.
        timer = 0f;

      

        // Stop the particles from playing if they were, then start the particles.
        gunParticles.Stop();
        gunParticles.Play();

        // Enable the line renderer and set it's first position to be the end of the gun.
        gunLine.enabled = true;
       // gunLine.SetPosition(0, transform.position);

        // Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        // Perform the raycast against gameobjects on the shootable layer and if it hits something...
        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            // Try and find an EnemyHealth script on the gameobject hit.
            Asteroid enemyHealth = shootHit.collider.GetComponent<Asteroid>();

            // If the EnemyHealth component exist...
            if (enemyHealth != null && enemyHealth.currentHealth != 0)
            {
                // ... the enemy should take damage.
                enemyHealth.TakeDamage(damagePerShot, shootHit.point, effectiveEfficiencyVal);
                
            }

            // Set the second position of the line renderer to the point the raycast hit.
          //  gunLine.SetPosition(1, shootHit.point);
        }
        // If the raycast didn't hit anything on the shootable layer...
        else
        {
            // ... set the second position of the line renderer to the fullest extent of the gun's range.
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
    }
}
