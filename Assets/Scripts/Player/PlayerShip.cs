using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Analytics;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.ParticleSystemJobs;
using System.Collections.Generic;

public class PlayerShip : MonoBehaviour
{
    // Movement
    [SerializeField] Rigidbody2D rig;
    private float movementSpeed = 0.7f;
    [SerializeField] Vector2 maxMovementSpeed;


    // Visual Stuff 
    [SerializeField] SpriteRenderer chassisRendererSprite;
    [SerializeField] SpriteRenderer lightsRendererSprite;
    [SerializeField] Light2D bodyLight;

    //Really don't like how I'm doing this right now, dumbbb

    [SerializeField] SpriteRenderer engineMainRendererSprite;
    [SerializeField] ParticleSystem engineMainParticles;
    [SerializeField] Light2D mainEngineLight;

    [SerializeField] SpriteRenderer engineLeftRendererSprite;
    [SerializeField] ParticleSystem engineLeftParticles;
    [SerializeField] Light2D leftEngineLight;

    [SerializeField] SpriteRenderer engineRightRendererSprite;
    [SerializeField] ParticleSystem engineRightParticles;
    [SerializeField] Light2D rightEngineLight;

    [SerializeField] SpriteRenderer engineFrontRightRendererSprite;
    [SerializeField] ParticleSystem engineFrontRightParticles;
    [SerializeField] Light2D frontRightEngineLight;

    [SerializeField] SpriteRenderer engineFrontLeftRendererSprite;
    [SerializeField] ParticleSystem engineFrontLeftParticles;
    [SerializeField] Light2D frontLeftEngineLight;

    // BETTER IMPLEMENTATION BUT NOT DONE YET
    [SerializeField] Engine[] leftFireEngines; // Engines on the right side
    [SerializeField] Engine[] rightFireEngines; // Engine on the left side
    [SerializeField] Engine[] mainFireEngines; // Main Big Engines/Engine

    private List<SpriteRenderer> engineSpriteRenderers = new List<SpriteRenderer>();

    // Shootings
    private bool canShoot = true;
    [SerializeField] GameObject projectilePrefab;

    // GAME VARIABLES
    [SerializeField] float shipHealth = 1f; // Keep between 1 and 0 and just either display * 100 or as a bar
    [SerializeField] float projectileDamage = 0.33f;

    public float GetProjectileDamage()
    {
        return projectileDamage;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Populating  lists
        engineSpriteRenderers.Add(engineRightRendererSprite);
        engineSpriteRenderers.Add(engineLeftRendererSprite);
        engineSpriteRenderers.Add(engineMainRendererSprite);
        engineSpriteRenderers.Add(engineFrontLeftRendererSprite);
        engineSpriteRenderers.Add(engineFrontRightRendererSprite);


        //Starting Coroutines
        StartCoroutine(ShootHandler());
        StartCoroutine(HandleLightingIntensity());

        Color shipColor = ScriptUtils.GetRandomColorFromSeed();

        lightsRendererSprite.color = shipColor;
        bodyLight.color = shipColor;

        engineFrontLeftRendererSprite. color = shipColor;
        engineFrontRightRendererSprite. color = shipColor;
        engineLeftRendererSprite. color = shipColor;
        engineRightRendererSprite. color = shipColor;
        engineMainRendererSprite. color = shipColor;
    }

    // Update is called once per frame
    void Update()
    {   
        // Handling movement
        if (Input.anyKey)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                rig.AddRelativeForceY(movementSpeed * shipHealth); //AAAAAAAAAAAAAA THIS WAS SO EASY IM SO DUMB
            }
            if (Input.GetKey(KeyCode.LeftArrow)) // I could cap these but I'm leaving them for now because they're funny
            {
                rig.AddRelativeForceY((movementSpeed / 3) * shipHealth); // small amount of movement forwards
                rig.angularVelocity += movementSpeed * shipHealth;
            }
            if (Input.GetKey(KeyCode.RightArrow)) // I could cap these but I'm leaving them for now because they're funny
            {
                 rig.AddRelativeForceY((movementSpeed /  3) * shipHealth); // small amount of movement forwards
                rig.angularVelocity -= movementSpeed * shipHealth;
            }
            if (Input.GetKey(KeyCode.Space)) // Slow down that sheep
            {
                if (rig.linearVelocity.magnitude > 0.01f)
                {
                    rig.linearVelocity *= 0.99f;
                }
                else 
                {
                    rig.linearVelocity = Vector2.zero;
                }
                if (rig.angularVelocity > 0.01f)
                {
                    rig.angularVelocity *= 0.99f;
                }
                else if (rig.angularVelocity < 0.01f)
                {
                    rig.angularVelocity *= 0.99f;
                }
                else 
                {
                    rig.angularVelocity = 0;
                }
            }
        }

        // Locking Movement Speeds - try to put this into the speed addition code later
        if (rig.linearVelocityY <= -maxMovementSpeed.y)
        {
            rig.linearVelocityY = -maxMovementSpeed.y;
        }
        if (rig.linearVelocityY >= maxMovementSpeed.y)
        {
            rig.linearVelocityY = maxMovementSpeed.y;
        }
        if (rig.linearVelocityX <= -maxMovementSpeed.x)
        {
            rig.linearVelocityX = -maxMovementSpeed.x;
        }
        if (rig.linearVelocityX >= maxMovementSpeed.x)
        {
            rig.linearVelocityX = maxMovementSpeed.x;
        }

        //Emission System Nonsense - tying it into engine alpha to make it easierrr

        var mainEngineParticlesMain = engineMainParticles.main;
        mainEngineParticlesMain.startColor = engineMainRendererSprite.color;

        var leftEngineParticlesMain = engineLeftParticles.main;
        leftEngineParticlesMain.startColor = engineLeftRendererSprite.color;

        var rightEngineParticlesMain = engineRightParticles.main;
        rightEngineParticlesMain.startColor = engineRightRendererSprite.color;

        var frontRightEngineParticlesMain = engineFrontRightParticles.main;
        frontRightEngineParticlesMain.startColor = engineFrontRightRendererSprite.color;

        var frontLeftEngineParticlesMain = engineFrontLeftParticles.main;
        frontLeftEngineParticlesMain.startColor = engineFrontLeftRendererSprite.color;

    }


    //INTERNAL METHODS
    private float AddWithMax(float floatToAddTo, float floatToAdd, float maxValue)
    {
        if ((floatToAddTo + floatToAdd) < maxValue)
        {
            return floatToAdd;
        }
        else 
        {
            return 0;
        }
    }

    private IEnumerator HandleLightingIntensity()
    {

        while (true)
        {
            Color lightsTempColor = lightsRendererSprite.color;  //We want the lights of the ship to light up more when Shooting and decrease when not
            Color engineRightTempColor = engineRightRendererSprite.color;
            Color engineLeftTempColor = engineLeftRendererSprite.color;
            Color engineMainTempColor = engineMainRendererSprite.color;
            Color engineFrontLeftTempColor = engineFrontLeftRendererSprite.color;
            Color engineFrontRightTempColor = engineFrontRightRendererSprite.color;


            if (Input.GetKey(KeyCode.Z))
            {
                lightsTempColor.a += AddWithMax(lightsTempColor.a, 0.01f, 1.5f);
            }
            else 
            {
                if (lightsTempColor.a - 0.01f >= 0)
                {
                    lightsTempColor.a -= (0.01f) / 15f ;
                }
                else 
                {
                    lightsTempColor.a = 0;
                }
            }

            if (Input.GetKey(KeyCode.LeftArrow)) // Right Engine for Moving Left
            {
                engineRightTempColor.a += AddWithMax(engineRightTempColor.a, 0.01f, 1.5f);
            }
            else 
            {
                if (engineRightTempColor.a - 0.01f >= 0)
                {
                    engineRightTempColor.a -= (0.05f) / 15f ;
                }
                else 
                {
                    engineRightTempColor.a = 0;
                }
            }

            if (Input.GetKey(KeyCode.RightArrow)) // Right Engine for Moving Left
            {
                engineLeftTempColor.a += AddWithMax(engineLeftTempColor.a, 0.01f, 1.5f);
            }
            else 
            {
                if (engineLeftTempColor.a - 0.01f >= 0)
                {
                    engineLeftTempColor.a -= (0.05f) / 15f ;
                }
                else 
                {
                    engineLeftTempColor.a = 0;
                }
            }

            if (Input.GetKey(KeyCode.UpArrow)) // Right Engine for Moving Left
            {
                engineMainTempColor.a += AddWithMax(engineMainTempColor.a, 0.01f, 1.5f);
            }
            else 
            {
                if (engineMainTempColor.a - 0.01f >= 0)
                {
                    engineMainTempColor.a -= (0.05f) / 15f ;
                }
                else 
                {
                    engineMainTempColor.a = 0;
                }
            }

            if (Input.GetKey(KeyCode.Space)) // Fire all Engines to slow down
            {
                engineMainTempColor.a += AddWithMax(engineMainTempColor.a, 0.01f, UnityEngine.Random.Range(1.0f, 1.5f));
                engineRightTempColor.a += AddWithMax(engineRightTempColor.a, 0.01f, UnityEngine.Random.Range(1.0f, 1.5f));
                engineLeftTempColor.a += AddWithMax(engineLeftTempColor.a, 0.01f, UnityEngine.Random.Range(1.0f, 1.5f));
                engineFrontLeftTempColor.a += AddWithMax(engineFrontLeftTempColor.a, 0.01f, UnityEngine.Random.Range(1.0f, 2f));
                engineFrontRightTempColor.a += AddWithMax(engineFrontRightTempColor.a, 0.01f, UnityEngine.Random.Range(1.0f, 2f));
            }
            else 
            {
                if (engineMainTempColor.a - 0.01f >= 0)
                {
                    engineMainTempColor.a -= (0.05f) / 15f ;
                }
                else 
                {
                    engineMainTempColor.a = 0;
                }

                 if (engineLeftTempColor.a - 0.01f >= 0)
                {
                    engineLeftTempColor.a -= (0.05f) / 15f ;
                }
                else 
                {
                    engineLeftTempColor.a = 0;
                }

                if (engineRightTempColor.a - 0.01f >= 0)
                {
                    engineRightTempColor.a -= (0.05f) / 15f ;
                }
                else 
                {
                    engineRightTempColor.a = 0;
                }

                if (engineFrontRightTempColor.a - 0.01f >= 0)
                {
                    engineFrontRightTempColor.a -= (0.05f) / 15f ;
                }
                else 
                {
                    engineFrontRightTempColor.a = 0;
                }

                if (engineFrontLeftTempColor.a - 0.01f >= 0)
                {
                    engineFrontLeftTempColor.a -= (0.05f) / 15f ;
                }
                else 
                {
                    engineFrontLeftTempColor.a = 0;
                }
            }

            lightsRendererSprite.color = lightsTempColor;
            bodyLight.color = lightsTempColor;

            engineLeftRendererSprite.color = engineLeftTempColor;
            leftEngineLight.color = engineLeftTempColor;

            engineRightRendererSprite.color = engineRightTempColor;
            rightEngineLight.color = engineRightTempColor;

            engineMainRendererSprite.color = engineMainTempColor;
            mainEngineLight.color = engineMainTempColor;

            engineFrontLeftRendererSprite.color = engineFrontLeftTempColor;
            frontLeftEngineLight.color = engineFrontLeftTempColor;

            engineFrontRightRendererSprite.color = engineFrontRightTempColor;
            frontRightEngineLight.color = engineFrontRightTempColor;

            yield return null;
        }
    }

    private IEnumerator ShootHandler()
    {
        while (canShoot)
        {
            //Shootings
            if (Input.GetKey(KeyCode.Z))
            {
                Vector2 offset = this.transform.up * 2; // 'up' is relative to the ship's rotation

                GameObject projectile = Instantiate(projectilePrefab, (Vector2) this.transform.position + offset,this.transform.localRotation); // Offset so it's not inside the ship

                //Change Projectile Color to match lights
                projectile.GetComponent<SpriteRenderer>().color = new Color (lightsRendererSprite.color.r, lightsRendererSprite.color.g, lightsRendererSprite.color.b, 1f);
                projectile.GetComponent<Light2D>().color = new Color (lightsRendererSprite.color.r, lightsRendererSprite.color.g, lightsRendererSprite.color.b, 1f);

                StartCoroutine(ScriptUtils.PositionLerp(projectile.transform, projectile.transform.position, this.transform.up * 1000,  20.5f - (rig.linearVelocityX / 10) - (rig.linearVelocityY / 10)));
            }
            yield return new WaitForSeconds(0.7f); // Cooldown
        }   
    }

    //EXTERNAL METHODS
    public void DecreaseHealth(float amount)
    {
        amount = Mathf.Abs(amount);

        if ((shipHealth - amount) > 0f)
        {
            shipHealth -= amount;
        }
        else
        {
            shipHealth = 0f;

            StartCoroutine(ScriptUtils.SlowTime(0f,10f));

            StartCoroutine(GameManager.GameOver());
        }
        Debug.Log("Player Damaged for : " + amount + " Damage");   
    }

    public float GetHealth()
    {
        return shipHealth;
    }

    public void SetColor(Color color)
    {

    }

}
