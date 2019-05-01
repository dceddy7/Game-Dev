using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour , IDestructible
{
    public float maxHealth;
    public float health;
    public SpriteRenderer healthBar;
    float healthbarSize;

    public void Die()
    {
        Destroy(this.gameObject);
        //throw new System.NotImplementedException();
    }

    public void Hit(int dmg)
    {

        health -= dmg;
        this.healthBar.gameObject.SetActive(true);
        
    }

    // Start is called before the first frame update
    void Start()
    {
        this.healthBar.gameObject.SetActive(false);
        healthbarSize = healthBar.size.x;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        float normalizedHealth = healthbarSize * (health / maxHealth);
        healthBar.size = new Vector2(normalizedHealth, healthBar.size.y);
        if (health <= 0)
        {
            Die();
        }
    }

}
