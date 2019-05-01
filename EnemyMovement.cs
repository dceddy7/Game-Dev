using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Author: Jacob Eddy
 * Created: 4/30
 * Last modified 5/1/2018 
 */
public class EnemyMovement : MonoBehaviour {


     GameObject playerObj;
     Transform Player;
    public bool isDead = false;
    public float MoveSpeed = 20;
    public float MaxDist = 200;
    public float MinDist = 100;
    public float range = 200f;
    public float speed = 1f;
    public float orbitSpeed = 20f;
    //private AudioSource gunshot;

    private EnemyHealth HealthScript;


    public int damagePerShot = 15;                  // The damage inflicted by each bullet.
    public float timeBetweenBullets = 3f;

    public float timer;
    public Ray shootRay;                                   // A ray from the gun end forwards.
    public RaycastHit shootHit;                            // A raycast hit to get information about what was hit.
    public int shootableMask;
    public float effectsDisplayTime = 0.2f;
    
    public LineRenderer gunLine;

    Vector3 rotationDir;

    void Awake()
    {
        rotationDir = new Vector3(Random.Range(.001f, 1f), Random.Range(.001f, 1f), Random.Range(.001f, 1f));
        speed = speed + Random.Range(-.2f, .2f);
        orbitSpeed = orbitSpeed + Random.Range(-10f, 10f);
        playerObj = GameObject.FindGameObjectWithTag("PlayerShip");
       // gunshot = GetComponent<AudioSource>();
        Player = playerObj.transform;
        // Create a layer mask for the Shootable layer.
        shootableMask = LayerMask.GetMask("Player");
        HealthScript = this.GetComponent<EnemyHealth>();

    }

    void FixedUpdate()
    {

        timer += Time.deltaTime;
        if (HealthScript.getDead() == false)
        { 
            Quaternion OriginalRot = transform.rotation;
            transform.LookAt(Player);
            Quaternion NewRot = transform.rotation;
            transform.rotation = OriginalRot;
            transform.rotation = Quaternion.Lerp(transform.rotation, NewRot, speed * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, Player.position) >= MinDist)
        {

            transform.position += transform.forward * MoveSpeed * Time.deltaTime;
        }
        if (Vector3.Distance(transform.position, Player.position) <= MaxDist)
        {
            transform.RotateAround(Player.position, rotationDir, orbitSpeed * Time.deltaTime);
            // If it's time to fire...
            if (timer >= timeBetweenBullets)
            {
                // ... shoot the gun.
                if (HealthScript.getDead() == false)
                {
                    Shoot();
                }
            }

            // If the timer has exceeded the proportion of timeBetweenBullets that the effects should be displayed for...
            if (timer >= timeBetweenBullets * effectsDisplayTime)
            {
                // ... disable the effects.
                DisableEffects();
            }
        }
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


        // Enable the line renderer and set it's first position to be the end of the gun.
        gunLine.enabled = true;
        

        // Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        // Perform the raycast against gameobjects on the shootable layer and if it hits something...
        if (!isDead)
        {
            if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
            {
                // Try and find an EnemyHealth script on the gameobject hit.
                Ship enemyHealth = shootHit.collider.GetComponent<Ship>();

                // If the EnemyHealth component exist...
                if (enemyHealth != null)
                {
                    // ... the enemy should take damage.
                 //   gunshot.Play();
                    Ship.ChangeHealthValue(-damagePerShot);
                    gunLine.SetPosition(1, transform.InverseTransformPoint(shootHit.point));
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
}
