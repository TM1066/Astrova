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


    // ASTEROID DESTRUCTION STUFF
    public Texture2D asteroidTexture; // Original texture
    public GameObject asteroidPrefab; // Prefab for the new asteroid pieces
    public int minSegments = 4;       // Minimum number of pieces
    public int maxSegments = 10;      // Maximum number of pieces

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("Projectile")) // Player projectile
        {   
            asteroidHealth -= collision.gameObject.GetComponent<Projectile>().GetDamage();

            if (asteroidHealth <= 0)
            {
                GameManager.IncrementCurrentScore();
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

        StartCoroutine(DestroyAsteroid());
    }

    private IEnumerator DestroyAsteroid()
    {
        this.GetComponent<SpriteRenderer>().color = Color.clear;

        StartCoroutine(ScriptUtils.DestroyGameObjectAfterTime(this.gameObject,3f));

        yield return null;
    }


    private void SplitAsteroid()
    {
        int segments = UnityEngine.Random.Range(minSegments, maxSegments + 1);

        float segmentWidth = asteroidTexture.width / Mathf.Sqrt(segments);
        float segmentHeight = asteroidTexture.height / Mathf.Sqrt(segments);

        for (int i = 0; i < Mathf.Sqrt(segments); i++)
        {
            for (int j = 0; j < Mathf.Sqrt(segments); j++)
            {
                CreateSegment(i, j, segmentWidth, segmentHeight);
            }
        }
    }

    private void CreateSegment(int xIndex, int yIndex, float segmentWidth, float segmentHeight)
    {
        // Calculate segment position
        float x = xIndex * segmentWidth;
        float y = yIndex * segmentHeight;

        // Create a new texture for the segment
        Texture2D segmentTexture = new Texture2D((int)segmentWidth, (int)segmentHeight);
        segmentTexture.SetPixels(asteroidTexture.GetPixels((int)x, (int)y, (int)segmentWidth, (int)segmentHeight));
        segmentTexture.Apply();

        // Create a sprite from the texture
        Sprite segmentSprite = Sprite.Create(segmentTexture, new Rect(0, 0, segmentWidth, segmentHeight), Vector2.one * 0.5f);

        // Instantiate the new GameObject
        GameObject newSegment = Instantiate(asteroidPrefab, transform.position, Quaternion.identity);
        newSegment.GetComponent<SpriteRenderer>().sprite = segmentSprite;

        // Assign random movement to the segment
        Rigidbody2D rb = newSegment.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;
            rb.AddForce(randomDirection * UnityEngine.Random.Range(50f, 100f)); // Adjust force as needed
        }
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
    void OnLevelWasLoaded(int level)
    {
       Destroy(gameObject);
    }
}
