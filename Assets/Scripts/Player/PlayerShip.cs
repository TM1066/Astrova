using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.ParticleSystemJobs;
using UnityEditor;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;

public class PlayerShip : MonoBehaviour
{
    // Movement
    [Header("Movement")]
    [SerializeField] Rigidbody2D rig;
    private float movementSpeed = 10f;
    [SerializeField] Vector2 maxMovementSpeed;
    
    [Header("Keys")]
    private KeyCode forwardsKey = GameManager.playerForwards;
    private KeyCode backwardsKey = GameManager.playerBackwards;
    private KeyCode leftKey = GameManager.playerLeft;
    private KeyCode rightKey = GameManager.playerRight;
    private KeyCode stopKey = GameManager.playerStop;
    private KeyCode fireKey = GameManager.playerFire;
    private KeyCode shieldKey = GameManager.playerShield;

    // Visual Stuff 
    [Header("Visuals")]
    [SerializeField] SpriteRenderer chassisRendererSprite;
    [SerializeField] SpriteRenderer lightsRendererSprite;
    [SerializeField] Light2D bodyLight;

    [SerializeField] Engine[] leftFireEngines; // Engines on the right side
    [SerializeField] Engine[] rightFireEngines; // Engine on the left side
    [SerializeField] Engine[] mainFireEngines; // Main Big Engines/Engine
    [SerializeField] Engine[] backFireEngines; // Engines on the front of the ship so you fire them to go backwards

    private List<Engine> allEngines = new List<Engine>();

    [SerializeField] ParticleSystem deathParticles;

    // Shootings
    [Header("Shooting")]
    private bool canShoot = true;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] List<Transform> projectileSpawnLocations = new List<Transform>();

    // Shieldings
    public Shield shield;

    // GAME VARIABLES
    [Header("Game Variables")]
    [SerializeField] float shipHealth = 1f; // Keep between 1 and 0 and just either display * 100 or as a bar
    [SerializeField] float minHealth = 0.15f;
    [SerializeField] float projectileDamage = 0.33f;
    private bool isDead = false;

    //POWERUPS
    private Dictionary<string, bool> powerUpDict = new Dictionary<string, bool>() { 
        {"Triple Shot", false}, 
        {"Infinite Shield", false},
        {"Invincible", false}
        };

    //Audio Stuff
    [Header ("Audio Stuff")]
    private AudioSource thisAudioPlayer; 
    [SerializeField] AudioClip fireAudio;
    [SerializeField] AudioClip hurtAudio;
    //[SerializeField] float maxEngineVolume = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Populating lists - AAAAAAAAA
        foreach (var engine in leftFireEngines)
        {
            if (allEngines.Contains(engine))
            {
                
            }
            else 
            {
                allEngines.Add(engine);
            }
        }
        foreach (var engine in rightFireEngines)
        {
            if (allEngines.Contains(engine))
            {
                
            }
            else 
            {
                allEngines.Add(engine);
            }
        }
        foreach (var engine in backFireEngines)
        {
            if (allEngines.Contains(engine))
            {
                
            }
            else 
            {
                allEngines.Add(engine);
            }
        }
        foreach (var engine in mainFireEngines)
        {
            if (allEngines.Contains(engine))
            {
                
            }
            else 
            {
                allEngines.Add(engine);
            }
        }

        thisAudioPlayer = GetComponent<AudioSource>();

        //Starting Coroutines
        StartCoroutine(ShootHandler());
        StartCoroutine(HandleLightingAndSoundIntensity());

        SetColor(GameManager.GetCurrentUserColor());
    }

    void FixedUpdate()
    {   
        // Handling movement
        if (Input.anyKey && !isDead)
        {
            if (Input.GetKey(forwardsKey) | Input.GetKey(GameManager.altPlayerForwards))
            {
                rig.AddRelativeForceY(movementSpeed * shipHealth); //AAAAAAAAAAAAAA THIS WAS SO EASY IM SO DUMB
            }
            if (Input.GetKey(backwardsKey))
            {
                rig.AddRelativeForceY(-(movementSpeed * shipHealth)); 
            }
            if (Input.GetKey(leftKey)) // I could cap these but I'm leaving them for now because they're funny
            {
                rig.AddRelativeForceY((movementSpeed / 3) * shipHealth); // small amount of movement forwards
                rig.angularVelocity += movementSpeed * shipHealth;
            }
            if (Input.GetKey(rightKey)) // I could cap these but I'm leaving them for now because they're funny
            {
                 rig.AddRelativeForceY((movementSpeed /  3) * shipHealth); // small amount of movement forwards
                rig.angularVelocity -= movementSpeed * shipHealth;
            }
            if (Input.GetKey(stopKey))// Slow down that sheep
            {
                if (rig.linearVelocity.magnitude > 0.01f)
                {
                    rig.linearVelocity *= 0.91f;
                }
                else 
                {
                    rig.linearVelocity = Vector2.zero;
                }
                if (rig.angularVelocity > 0.01f)
                {
                    rig.angularVelocity *= 0.91f;
                }
                else if (rig.angularVelocity < 0.01f)
                {
                    rig.angularVelocity *= 0.91f;
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

        foreach (var engine in allEngines)
        {
            // HANDLING EMISSIONS
            var EngineParticlesMain = engine.particleEmitter.main;

            Color particleColor = Color.Lerp(CalculateColorBasedOnHealth(engine.spriteRenderer.color, Color.red),GameManager.GetCurrentUserColorFullAlpha(), UnityEngine.Random.Range(0f,1f));
            particleColor.a = engine.spriteRenderer.color.a;

            EngineParticlesMain.startColor = engine.spriteRenderer.color; // have Engines shoot out more red Color as ship gets damaged


            //HANDLING VOLUME
            engine.audioSource.volume = engine.spriteRenderer.color.a *  engine.audioVolumeModifier * 0.05f;
            
        }

        if (isDead)
        {
            foreach (var engine in allEngines) // clears all engine acivity if player is dead
            {
                engine.audioSource.volume = 0f;
                engine.spriteRenderer.color = Color.clear;

                var EngineParticlesMain = engine.particleEmitter.main;
                EngineParticlesMain.startColor = Color.clear;
            }
        }
    }

    void Update()
    {
        if (Input.anyKey && !isDead)
        {
            if (Input.GetKeyDown(shieldKey))
            {
                shield.ActiDevate();
            }
        }
    }

    //INTERNAL METHODS

    private Color CalculateColorBasedOnHealth(Color startColor, Color endColor)
    {
        return new Color (Color.Lerp(endColor, startColor, shipHealth).r, Color.Lerp(endColor, startColor, shipHealth).g, Color.Lerp(endColor, startColor, shipHealth).b, startColor.a); // weird but works & retains start alpha
    }

    private IEnumerator HandleLightingAndSoundIntensity()
    {
            
        while (true && !isDead)
        {
            List<Color> rightFireEnginesColors = new List<Color>();
            List<Color> leftFireEnginesColors = new List<Color>();
            List<Color> mainFireEnginesColors = new List<Color>();
            List<Color> backFireEnginesColors = new List<Color>();

            foreach (var engine in rightFireEngines)
            {
                rightFireEnginesColors.Add(engine.spriteRenderer.color);
            }
            foreach (var engine in leftFireEngines)
            {
                leftFireEnginesColors.Add(engine.spriteRenderer.color);
            }
            foreach (var engine in mainFireEngines)
            {
                mainFireEnginesColors.Add(engine.spriteRenderer.color);
            }
            foreach (var engine in backFireEngines)
            {
                backFireEnginesColors.Add(engine.spriteRenderer.color);
            }


            Color lightsTempColor = lightsRendererSprite.color;  //We want the lights of the ship to light up more when Shooting and decrease when not
            
            Color engineRightTempColor = ScriptUtils.GetAverageColor(rightFireEnginesColors);
            Color engineRightHealthColor = CalculateColorBasedOnHealth(engineRightTempColor, Color.red);
            Color engineLeftTempColor = ScriptUtils.GetAverageColor(leftFireEnginesColors);
            Color engineLeftHealthColor = CalculateColorBasedOnHealth(engineLeftTempColor, Color.red);
            Color engineMainTempColor = mainFireEnginesColors[0];
            Color engineMainHealthColor = CalculateColorBasedOnHealth(engineMainTempColor , Color.red);
            Color engineBackTempColor = ScriptUtils.GetAverageColor(backFireEnginesColors);
            Color engineBackHealthColor = CalculateColorBasedOnHealth(engineBackTempColor, Color.red);

            if (Input.GetKey(leftKey)) // Right Engines for Moving Left
            {
                engineRightTempColor.a += ScriptUtils.AddWithMax(engineRightTempColor.a, 0.01f, 1.5f);
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

            if (Input.GetKey(rightKey)) // LeftEngines for Moving Right
            {
                engineLeftTempColor.a += ScriptUtils.AddWithMax(engineLeftTempColor.a, 0.01f, 1.5f);
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

            if (Input.GetKey(forwardsKey) | Input.GetKey(GameManager.altPlayerForwards))// Back engines for moving forwards
            {
                engineMainTempColor.a += ScriptUtils.AddWithMax(engineMainTempColor.a, 0.01f, 1.5f);
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

            if (Input.GetKey(backwardsKey)) // Front Engines for moving Back
            {
                engineBackTempColor.a += ScriptUtils.AddWithMax(engineBackTempColor.a, 0.01f, 1.5f);
            }
            else 
            {
                if (engineBackTempColor.a - 0.01f >= 0)
                {
                    engineBackTempColor.a -= (0.05f) / 15f ;
                }
                else 
                {
                    engineBackTempColor.a = 0;
                }
            }

            if (Input.GetKey(stopKey)) // Fire all Engines to slow down
            {
                engineMainTempColor.a += ScriptUtils.AddWithMax(engineMainTempColor.a, 0.01f, UnityEngine.Random.Range(1.0f, 1.5f));
                engineRightTempColor.a += ScriptUtils.AddWithMax(engineRightTempColor.a, 0.01f, UnityEngine.Random.Range(1.0f, 1.5f));
                engineLeftTempColor.a += ScriptUtils.AddWithMax(engineLeftTempColor.a, 0.01f, UnityEngine.Random.Range(1.0f, 1.5f));
                engineBackTempColor.a += ScriptUtils.AddWithMax(engineBackTempColor.a, 0.01f, UnityEngine.Random.Range(1.0f, 1.5f));
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

                if (engineBackTempColor.a - 0.01f >= 0)
                {
                    engineBackTempColor.a -= (0.05f) / 15f ;
                }
                else 
                {
                    engineBackTempColor.a = 0;
                }
            }

            if (Input.GetKey(fireKey)) // bring up lights while firiing
            {
                lightsTempColor.a += ScriptUtils.AddWithMax(lightsTempColor.a, 0.1f, 1.5f);
            }
            else 
            {
                if (lightsTempColor.a - 0.01f >= 0)
                {
                    lightsTempColor.a -= (0.05f) / 15f ;
                }
                else 
                {
                    lightsTempColor.a = 0;
                }
            }

            // lightsRendererSprite.color = CalculateColorBasedOnHealth(lightsTempColor, new Color(lightsRendererSprite.color.r, lightsRendererSprite.color.g, lightsRendererSprite.color.b,0f));
            // if (lightsRendererSprite.color.a <= 0.25f)
            // {
            //     canShoot = true;
            // }
            // bodyLight.color = CalculateColorBasedOnHealth(lightsTempColor, Color.clear);

            lightsRendererSprite.color = lightsTempColor;
            bodyLight.color = lightsTempColor;


            foreach (var engine in leftFireEngines)
            {
                engine.spriteRenderer.color = engineLeftTempColor;
                engine.lightSource.color = engineLeftTempColor;
            }
            foreach (var engine in rightFireEngines)
            {
                engine.spriteRenderer.color = engineRightTempColor;
                engine.lightSource.color = engineRightTempColor;
            }
            foreach (var engine in mainFireEngines)
            {
                engine.spriteRenderer.color = engineMainTempColor;
                engine.lightSource.color = engineMainTempColor;
            }
            foreach (var engine in backFireEngines)
            {
                engine.spriteRenderer.color = engineBackTempColor;
                engine.lightSource.color = engineBackTempColor;
            }

            yield return null;
        }
    }

    private IEnumerator ShootHandler()
    {
        while (gameObject)
        {
            //Shootings
            if (Input.GetKey(fireKey) && !isDead && canShoot)
            {

                if (powerUpDict["Triple Shot"])
                {
                    foreach (Transform spawnTransform in projectileSpawnLocations)
                    {
                        GameObject projectile = Instantiate(projectilePrefab, spawnTransform.position, this.transform.localRotation); // Offset so it's not inside the ship

                        projectile.GetComponent<Projectile>().SetDamage(projectileDamage);

                        //Change Projectile Color to match lights
                        projectile.GetComponent<SpriteRenderer>().color = new Color (lightsRendererSprite.color.r, lightsRendererSprite.color.g, lightsRendererSprite.color.b, 1f);
                        projectile.GetComponent<Light2D>().color = new Color (lightsRendererSprite.color.r, lightsRendererSprite.color.g, lightsRendererSprite.color.b, 50f);

                        float velocityMagnitude = Mathf.Sqrt(rig.linearVelocityX * rig.linearVelocityX + rig.linearVelocityY * rig.linearVelocityY);
                        float adjustedDuration = Mathf.Clamp(20.5f - (velocityMagnitude / 10), 10f, 20.5f);

                        StartCoroutine(ScriptUtils.PositionLerp(projectile.transform, projectile.transform.position, spawnTransform.up.normalized * 1000,  adjustedDuration));
                        
                    }
                    ScriptUtils.PlaySound(null, fireAudio);
                }
                else 
                {
                    Vector2 offset = this.transform.up * 2; // 'up' is relative to the ship's rotation

                    GameObject projectile = Instantiate(projectilePrefab, (Vector2) this.transform.position + offset, this.transform.localRotation); // Offset so it's not inside the ship

                    projectile.GetComponent<Projectile>().SetDamage(projectileDamage);

                    //Change Projectile Color to match lights
                    projectile.GetComponent<SpriteRenderer>().color = new Color (lightsRendererSprite.color.r, lightsRendererSprite.color.g, lightsRendererSprite.color.b, 1f);
                    projectile.GetComponent<Light2D>().color = new Color (lightsRendererSprite.color.r, lightsRendererSprite.color.g, lightsRendererSprite.color.b, 50f);

                    float velocityMagnitude = Mathf.Sqrt(rig.linearVelocityX * rig.linearVelocityX + rig.linearVelocityY * rig.linearVelocityY);
                    float adjustedDuration = Mathf.Clamp(20.5f - (velocityMagnitude / 10), 10f, 20.5f);

                    StartCoroutine(ScriptUtils.PositionLerp(projectile.transform, projectile.transform.position, this.transform.up.normalized * 1000,  adjustedDuration));

                    ScriptUtils.PlaySound(null, fireAudio);
                }
            }

            else if (Input.GetKey(fireKey) && !canShoot)
            {
                UiUtils.ShowMessage("Can't Shoot","Your Weapons are offline!",new Vector2(200,200),false);
            }
            canShoot = false;
            yield return new WaitForSeconds(0.7f); // Cooldown
            canShoot = true;
        }   
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Evil Projectile"))
        {
            Debug.Log("Player Shot!!!");
            DecreaseHealth(Mathf.Abs(other.GetComponent<Projectile>().GetDamage()));
            Destroy(other.gameObject);
        }
    }

    //EXTERNAL METHODS
    public void DecreaseHealth(float amount)
    {
        amount = Mathf.Abs(amount);

        if ((shipHealth - amount) > minHealth)
        {
            shipHealth -= amount;
            ScriptUtils.PlaySound(thisAudioPlayer, hurtAudio);
        }
        else if (isDead) // don't kill if already dead
        {
        }
        else 
        {
            shipHealth = 0f;

            isDead = true;
            
            var colorParticleThing = deathParticles.main;

            colorParticleThing.startColor = Color.red; // enable and make em red

            StartCoroutine(GameManager.GameOver());
        }
        Debug.Log("Player Damaged for : " + amount + " Damage");   
    }

    public void IncreaseHealth(float amount)
    {
        amount = Mathf.Abs(amount);

        if ((shipHealth + amount) <= 1f)
        {
            shipHealth += amount;
        }
        else
        {
            shipHealth = 1f;
        }
    }

    public float GetHealth()
    {
        return shipHealth;
    }

    public float GetMinHealth()
    {
        return minHealth;
    }

    public bool GetIsDead()
    {
        return isDead;
    }

    public void SetColor(Color color)
    {

        if (GameManager.shipColorTags.Count >= 1) // Special color handling
        {
            switch (GameManager.shipColorTags[0])
            {
                case "spk":
                    lightsRendererSprite.color = Color.black;
                    bodyLight.color = Color.black;
                    chassisRendererSprite.color = Color.black;
                    foreach (var engine in allEngines)
                    {
                        engine.spriteRenderer.color = Color.black;
                    }
                    break;
                case "elf":
                    Color lightRed = new Color(1,0.191f,0.262f);
                    Color lightGreen = new Color(0.6265259f,1,0.56f);


                    lightsRendererSprite.color = lightRed;
                    bodyLight.color = lightRed;
                    chassisRendererSprite.color = lightGreen;
                    foreach (var engine in allEngines)
                    {
                        engine.spriteRenderer.color = lightGreen;
                    }
                    break;
            }
        }

        else 
        {
            lightsRendererSprite.color = new Color (color.r, color.g, color.b,1.5f);
            bodyLight.color = color;

            if (GameManager.GetColorfulShipsEnabled())
            {
                bodyLight.color = new Color (ScriptUtils.GetComplimentaryColor(color).r, ScriptUtils.GetComplimentaryColor(color).g, ScriptUtils.GetComplimentaryColor(color).b, 1f);
                chassisRendererSprite.color = new Color (ScriptUtils.GetComplimentaryColor(color).r, ScriptUtils.GetComplimentaryColor(color).g, ScriptUtils.GetComplimentaryColor(color).b, 1f);

            }

            foreach (var engine in allEngines)
            {
                engine.spriteRenderer.color = color;
            }
        }
        GameManager.shipColorTags.Clear();
    }

    private IEnumerator ActivatePowerUp(string powerUpKey)
    {
        powerUpDict[powerUpKey] = true; 
        Debug.Log("Power up: " + powerUpKey + " enabled");
        yield return new WaitForSeconds(6); // power up duration no workinggg
        powerUpDict[powerUpKey] = false;
        Debug.Log("Power up: " + powerUpKey + " disabled");
    }

    public void ActivatePowerUpHandler(string powerUpKey)
    {
        StartCoroutine(ActivatePowerUp(powerUpKey));

    }

    public bool CheckPowerUp(string powerUpKey)
    {
        return powerUpDict[powerUpKey];
    }
}
