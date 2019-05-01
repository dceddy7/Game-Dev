using UnityEngine;
/*
 * Author: Jacob Eddy
 * Created: 4/30
 * Last modified 5/1/2018 
 */
public class Asteroid : MonoBehaviour
{
    public GameObject self;
    public int ResourceValue = 1000;            // The amount of health the enemy starts the game with.
    public int ResourceType;                    //the type of resource in the asteroid (0 = iron, 1 = gold, 2 = silicon)
    public int currentHealth;                   // The current health the enemy has.
    //public float sinkSpeed = 2.5f;              // The speed at which the enemy sinks through the floor when dead.
    public int scoreValue = 10;                 // The amount added to the player's score when the enemy dies.
    //public AudioClip deathClip;                 // The sound to play when the enemy dies.


    //Animator anim;                              // Reference to the animator.
   // AudioSource enemyAudio;                     // Reference to the audio source.
    public ParticleSystem hitParticles;                // Reference to the particle system that plays when the enemy is damaged.
    SphereCollider boxCollider;            // Reference to the capsule collider.
    bool isDead;                                // Whether the enemy is dead.
    //bool isSinking;                             // Whether the enemy has started sinking through the floor.


    void Awake()
    {
        // Setting up the references.
        //anim = GetComponent<Animator>();
        //enemyAudio = GetComponent<AudioSource>();
       // hitParticles = GetComponentInChildren<ParticleSystem>();
        boxCollider = GetComponent<SphereCollider>();

        // Setting the current health when the enemy first spawns.
        currentHealth = ResourceValue;
    }

    void Update()
    {
      // if (isDead) Destroy(self);
    }


    public void TakeDamage(int amount, Vector3 hitPoint, int efficiencyVal)
    {
        // If the enemy is dead...
        if (isDead)
            // ... no need to take damage so exit the function.
            return;

        
        // Reduce the current health by the amount of damage sustained.
        currentHealth -= amount;
        switch (ResourceType)
        {
            case 0:
                {
                    Ship.resources += efficiencyVal;
                    break;
                }
            case 1:
                {
                    Ship.resource2 += efficiencyVal;
                    break;
                }
            case 2:
                {
                    Ship.resource3 += efficiencyVal;
                    break;
                }

            default:
                break;
        }
        
        // Set the position of the particle system to where the hit was sustained.
        hitParticles.transform.position = hitPoint;

        // And play the particles.
        hitParticles.Play();

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
        isDead = true;
        gameObject.tag = "Untagged";
        //Debug.Log("Game object tag is: " + gameObject.tag);
        // Turn the collider into a trigger so shots can pass through it.
        boxCollider.isTrigger = true;

        //anim.SetTrigger("Death");

        // Change the audio clip of the audio source to the death clip and play it (this will stop the hurt clip playing).
        //enemyAudio.clip = deathClip;
        //enemyAudio.Play();

        Destroy(gameObject, 10f);
        //Destroy(this.gameObject);
    }

}