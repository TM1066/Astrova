using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

public class Asteroid : MonoBehaviour
{
    private float asteroidHealth = 2f;

    private bool canDamagePlayer;

    public bool IsVisible;

    private ParticleSystem destructionParticleSystem;

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("Projectile")) // Player projectile
        {   
            asteroidHealth -= collision.gameObject.GetComponent<Projectile>().GetDamage();

            if (asteroidHealth <= 0)
            {
                GameManager.IncrementScore();
               DestroyThis();
            } 

            GameObject.Destroy(collision.GameObject());
        }
         else if (collision.gameObject.CompareTag("Evil Projectile")) 
        {   
            asteroidHealth -= collision.gameObject.GetComponent<Projectile>().GetDamage();

            if (asteroidHealth <= 0)
            {
               DestroyThis();
            } 

            GameObject.Destroy(collision.GameObject());
        }
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.CompareTag("Player") && !canDamagePlayer)
        {
            TriggerCooldown(5f);

            // Actual Damage Calculation
            float damage = Mathf.Abs((0.1f * this.transform.localScale.x) * (other.gameObject.GetComponent<Rigidbody2D>().linearVelocityX + other.gameObject.GetComponent<Rigidbody2D>().linearVelocityY)) / 40; 
            // Rounding to 2dp
            damage = float.Parse(damage.ToString("n2"));

            other.gameObject.GetComponent<PlayerShip>().DecreaseHealth(damage); // Change how much damage depending on Size
        }    
    }

    void TriggerCooldown(float cooldownDuration)
    {
        StartCoroutine(ScriptUtils.BooleanDelay(
            () => canDamagePlayer,         // Getter: how to read the bool
            value => canDamagePlayer = value, // Setter: how to modify the bool
            cooldownDuration));      // Duration of the cooldown
    }

    private void DestroyThis() // blows up asteroid
    {
        destructionParticleSystem.Emit(50);

        GameObject.Destroy(this.gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Set Variables
        destructionParticleSystem = this.GetComponent<ParticleSystem>();

        // Randomly change size
        float scaleRandomChange = UnityEngine.Random.Range(0.0f, 0.8f);

        this.transform.localScale = new Vector3(this.transform.localScale.x - scaleRandomChange, this.transform.localScale.y - scaleRandomChange);
        asteroidHealth -= scaleRandomChange; // also scale health with asteroid size
    }

    private void OnApplicationQuit()
    {
        Destroy(gameObject);
    }
}
