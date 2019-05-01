using UnityEngine;
/*
 * Author: Jacob Eddy
 * Created: 4/30
 * Last modified 5/1/2018 
 */
public class EnemyHealth : MonoBehaviour
{
    public GameObject self;
    public int startingHealth = 100;            // The amount of health the enemy starts the game with.
    public int currentHealth;                   // The current health the enemy has.
    //public float sinkSpeed = 2.5f;              // The speed at which the enemy sinks through the floor when dead.
    public int scoreValue = 10;                 // The amount added to the player's score when the enemy dies.
    //public AudioClip deathClip;                 // The sound to play when the enemy dies.
    public int resourceLoot;
    public int fuelLoot;

    Animator anim;                              // Reference to the animator.
    //AudioSource enemyAudio;                     // Reference to the audio source.
    public ParticleSystem hitParticles;                // Reference to the particle system that plays when the enemy is damaged.
    BoxCollider boxCollider;            // Reference to the capsule collider.
    public bool isDead;                                // Whether the enemy is dead.
    //bool isSinking;                             // Whether the enemy has started sinking through the floor.


    void Awake()
    {
        // Setting up the references.
        anim = GetComponent<Animator>();
        //enemyAudio = GetComponent<AudioSource>();
       // hitParticles = GetComponentInChildren<ParticleSystem>();
        boxCollider = GetComponent<BoxCollider>();

        // Setting the current health when the enemy first spawns.
        currentHealth = startingHealth;
    }

    void Update()
    {

      // if (isDead) Destroy(self);
    }


    public void TakeDamage(int amount, Vector3 hitPoint)
    {
        // If the enemy is dead...
        Debug.Log(amount);
        if (isDead)
            // ... no need to take damage so exit the function.
            return;

        // Play the hurt sound effect.
        //enemyAudio.Play();

        // Reduce the current health by the amount of damage sustained.
        Debug.Log(amount);
        currentHealth -= amount;

        if (hitPoint != null)
        {
            // Set the position of the particle system to where the hit was sustained.
            hitParticles.transform.position = hitPoint;

            // And play the particles.
            hitParticles.Play();
        }

        // If the current health is less than or equal to zero...
        if (currentHealth <= 0)
        {
            // ... the enemy is dead.
            Death();
        }
    }


    void Death()
    {
        
        // The enemy is dead.
        setDead(true);
        Ship.resource4 += resourceLoot;
        Ship.fuel += fuelLoot;
        gameObject.tag = "Untagged";
       
        // Turn the collider into a trigger so shots can pass through it.
        boxCollider.isTrigger = true;

        if(anim != null)
            anim.SetTrigger("Death");

        

        Destroy(self, 10f);
        
    }

    public bool getDead()
    {
        return isDead;
    }

    public void setDead(bool val)
    {
        isDead = val;
    }

}