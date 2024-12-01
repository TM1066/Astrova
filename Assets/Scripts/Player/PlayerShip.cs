using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.ParticleSystemJobs;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;

public class PlayerShip : MonoBehaviour
{
    // Movement
    [SerializeField] Rigidbody2D rig;
    private float movementSpeed = 10f;
    [SerializeField] Vector2 maxMovementSpeed;


    // Visual Stuff 
    [SerializeField] SpriteRenderer chassisRendererSprite;
    [SerializeField] SpriteRenderer lightsRendererSprite;
    [SerializeField] Light2D bodyLight;

    [SerializeField] Engine[] leftFireEngines; // Engines on the right side
    [SerializeField] Engine[] rightFireEngines; // Engine on the left side
    [SerializeField] Engine[] mainFireEngines; // Main Big Engines/Engine
    [SerializeField] Engine[] backFireEngines; // Engines on the front of the ship so you fire them to go backwards

    private List<Engine> allEngines = new List<Engine>();

    // Shootings
    private bool canShoot = true;
    [SerializeField] GameObject projectilePrefab;

    // GAME VARIABLES
    [SerializeField] float shipHealth = 1f; // Keep between 1 and 0 and just either display * 100 or as a bar
    [SerializeField] float minHealth = 0.07f;
    [SerializeField] float projectileDamage = 0.33f;

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

        //Starting Coroutines
        StartCoroutine(ShootHandler());
        StartCoroutine(HandleLightingIntensity());

        SetColor(GameManager.GetCurrentUserColor());
    }

    void FixedUpdate()
    {   
        // Handling movement
        if (Input.anyKey)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                rig.AddRelativeForceY(movementSpeed * shipHealth); //AAAAAAAAAAAAAA THIS WAS SO EASY IM SO DUMB
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                rig.AddRelativeForceY(-(movementSpeed * shipHealth)); 
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
            var EngineParticlesMain = engine.particleEmitter.main;

            Color particleColor = Color.Lerp(CalculateColorBasedOnHealth(engine.spriteRenderer.color, Color.red),GameManager.GetCurrentUserColorFullAlpha(), UnityEngine.Random.Range(0f,1f));
            particleColor.a = engine.spriteRenderer.color.a;

            EngineParticlesMain.startColor = engine.spriteRenderer.color; // have Engines shoot out more red Color as ship gets damaged
        }
    }

    //INTERNAL METHODS
    private Color CalculateColorBasedOnHealth(Color startColor, Color endColor)
    {
        return new Color (Color.Lerp(endColor, startColor, shipHealth).r, Color.Lerp(endColor, startColor, shipHealth).g, Color.Lerp(endColor, startColor, shipHealth).b, startColor.a); // weird but works & retains start alpha
    }

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

            if (Input.GetKey(KeyCode.DownArrow)) // Right Engine for Moving Left
            {
                engineBackTempColor.a += AddWithMax(engineBackTempColor.a, 0.01f, 1.5f);
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

            if (Input.GetKey(KeyCode.Space)) // Fire all Engines to slow down
            {
                engineMainTempColor.a += AddWithMax(engineMainTempColor.a, 0.01f, UnityEngine.Random.Range(1.0f, 1.5f));
                engineRightTempColor.a += AddWithMax(engineRightTempColor.a, 0.01f, UnityEngine.Random.Range(1.0f, 1.5f));
                engineLeftTempColor.a += AddWithMax(engineLeftTempColor.a, 0.01f, UnityEngine.Random.Range(1.0f, 1.5f));
                engineBackTempColor.a += AddWithMax(engineBackTempColor.a, 0.01f, UnityEngine.Random.Range(1.0f, 1.5f));
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

            lightsRendererSprite.color = CalculateColorBasedOnHealth(lightsTempColor, new Color(lightsRendererSprite.color.r, lightsRendererSprite.color.g, lightsRendererSprite.color.b,0f));
            if (lightsRendererSprite.color.a <= 0.25f)
            {
                canShoot = true;
            }
            bodyLight.color = CalculateColorBasedOnHealth(lightsTempColor, Color.clear);


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
            if (Input.GetKey(KeyCode.Z) && canShoot)
            {
                Vector2 offset = this.transform.up * 2; // 'up' is relative to the ship's rotation

                GameObject projectile = Instantiate(projectilePrefab, (Vector2) this.transform.position + offset,this.transform.localRotation); // Offset so it's not inside the ship

                projectile.GetComponent<Projectile>().SetDamage(projectileDamage);

                //Change Projectile Color to match lights
                projectile.GetComponent<SpriteRenderer>().color = new Color (lightsRendererSprite.color.r, lightsRendererSprite.color.g, lightsRendererSprite.color.b, 1f);
                projectile.GetComponent<Light2D>().color = new Color (lightsRendererSprite.color.r, lightsRendererSprite.color.g, lightsRendererSprite.color.b, 1f);

                StartCoroutine(ScriptUtils.PositionLerp(projectile.transform, projectile.transform.position, this.transform.up * 1000,  20.5f - (rig.linearVelocityX / 10) - (rig.linearVelocityY / 10)));
            }
            else if (Input.GetKey(KeyCode.Z) && !canShoot)
            {
                UiUtils.ShowMessage("Can't Shoot","Your Weapons are offline!",new Vector2(200,200),false);
            }
            yield return new WaitForSeconds(0.5f); // Cooldown
        }   
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Evil Projectile"))
        {
            DecreaseHealth(Mathf.Abs(other.GetComponent<Projectile>().GetDamage()));
            Destroy(other);
        }
    }

    //EXTERNAL METHODS
    public void DecreaseHealth(float amount)
    {
        amount = Mathf.Abs(amount);

        if ((shipHealth - amount) > minHealth)
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

    public float GetMinHealth()
    {
        return minHealth;
    }

    public void SetColor(Color color)
    {
        lightsRendererSprite.color = new Color (color.r, color.g, color.b,1.5f);
        bodyLight.color = color;

        foreach (var engine in allEngines)
        {
            engine.spriteRenderer.color = color;
        }
    }
}
