using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Author: Jacob Eddy
 * Created: 4/30
 * Last modified 5/1/2018 
 */
public class ShootEnemy : MonoBehaviour {

    public int damagePerShot = 20;                  // The damage inflicted by each bullet.
    public float timeBetweenBullets = 0.15f;        // The time between each shot.
    public float range = 100f;                      // The distance the gun can fire.

    public float timer;                                    // A timer to determine when to fire.
    public Ray shootRay;                                   // A ray from the gun end forwards.
    public RaycastHit shootHit;                            // A raycast hit to get information about what was hit.
    public int shootableMask;                              // A layer mask so the raycast only hits things on the shootable layer.
    public ParticleSystem gunParticles;                    // Reference to the particle system.
    public LineRenderer gunLine;                           // Reference to the line renderer.
   // public AudioSource gunAudio;                           // Reference to the audio source.
   // public Light gunLight;                                 // Reference to the light component.
    public float effectsDisplayTime = 0.2f;                // The proportion of the timeBetweenBullets that the effects will display for.

    private Ship thePlayer; //A reference to the Ship, which is used to update the effective timers, range, and damage.
    public int identifier; //The identifier of the ship. 1 is cannon, 2 is railgun, 3 is laser, 4 is mining.
    public float effectiveTimer = 0f; //The firing rate of the turret.
    private float effectiveRange = 0f;
    private int effectiveDamage = 0; 

    void Awake()
    {
        // Create a layer mask for the Shootable layer.
        shootableMask = LayerMask.GetMask("Shootable");
        thePlayer = GameObject.Find("PlayerShipOuterShell").GetComponent<Ship>();
        UpdateAllEffectiveComponents();
       // Debug.Log("Effective range is: " + effectiveDamage);

        // Set up the references.
        // gunParticles = GetComponent<ParticleSystem>();
        // gunLine = GetComponent<LineRenderer>();
        // gunAudio = GetComponent<AudioSource>();
        // gunLight = GetComponent<Light>();
    }

    void Update()
    {
        timer += Time.deltaTime;
      //  if (thePlayer.hasBeenModified == true)
      //  {
            UpdateAllEffectiveComponents();
        //Debug.Log(effectiveRange);
        // }
      //  Debug.Log("Effective range is: " + effectiveDamage);
    }

    public void UpdateAllEffectiveComponents()
    {
        switch (identifier)
        {
            case 1:
                effectiveTimer = timeBetweenBullets * thePlayer.cannonRate;
                effectiveRange = range * thePlayer.cannonRange;
                effectiveDamage = (int) Mathf.Ceil(damagePerShot * thePlayer.cannonDamage);
                break;
            case 2:
                effectiveTimer = timeBetweenBullets * thePlayer.railgunRate;
                effectiveRange = range * thePlayer.railgunRange;
                effectiveDamage = (int) Mathf.Ceil(damagePerShot * thePlayer.railgunDamage);
                break;
            case 3:
                effectiveTimer = timeBetweenBullets * thePlayer.laserRate;
                effectiveRange = range * thePlayer.laserRange;
                effectiveDamage = (int) Mathf.Ceil(damagePerShot * thePlayer.laserDamage);
                break;

        }
    }

    public void DisableEffects()
    {
        // Disable the line renderer and the light.
        gunLine.enabled = false;
    //    gunLight.enabled = false;
    }

    public void Shoot(GameObject defaultEnemy)
    {
        // Reset the timer.
        timer = 0f;

        // Play the gun shot audioclip.
     //   gunAudio.Play();

        // Enable the light.
     //   gunLight.enabled = true;

        // Stop the particles from playing if they were, then start the particles.
        gunParticles.Stop();
        gunParticles.Play();

        // Enable the line renderer and set it's first position to be the end of the gun.
        gunLine.enabled = true;
       // gunLine.SetPosition(0, transform.position);

        // Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        EnemyHealth enemyHealth;

        // Perform the raycast against gameobjects on the shootable layer and if it hits something...
        if (Physics.Raycast(shootRay, out shootHit, effectiveRange, shootableMask))
        {
            // Try and find an EnemyHealth script on the gameobject hit.
            enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
        }
        else
        {
            print("Raycast failed, defaulting to closest enemy");
            enemyHealth = defaultEnemy.GetComponent<EnemyHealth>();
        }
        
        // If the EnemyHealth component exist...
        if (enemyHealth != null)
        {
            // ... the enemy should take damage.
            Debug.Log(enemyHealth.name);
            enemyHealth.TakeDamage(effectiveDamage, shootHit.point);
        }

        // Set the second position of the line renderer to the point the raycast hit.
        //  gunLine.SetPosition(1, shootHit.point);

        // If the raycast didn't hit anything on the shootable layer...
        else
        {
            // ... set the second position of the line renderer to the fullest extent of the gun's range.
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * effectiveRange);
        }
    }
}
