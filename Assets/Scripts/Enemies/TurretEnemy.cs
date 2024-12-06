using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class TurretEnemy : MonoBehaviour
{
    private GameObject player;

    [SerializeField] SpriteRenderer chassisSpriteRenderer;
    [SerializeField] SpriteRenderer lightsSpriteRenderer;
    [SerializeField] Light2D bodyLight;
    private Color thisColor;

    [SerializeField] float angleOffset;

    [SerializeField] GameObject projectilePrefab;

    [SerializeField] float health = 1f;

    //[SerializeField] float projectileDamage = 0.22f;

    private bool isFiring;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Setting up Variables
        player = GameObject.Find("SpaceShip");
        chassisSpriteRenderer.color = ScriptUtils.GetComplimentaryColor(GameManager.GetCurrentUserColorFullAlpha());
        thisColor = chassisSpriteRenderer.color;

        lightsSpriteRenderer = transform.GetChild(2).GetComponent<SpriteRenderer>();
        lightsSpriteRenderer.color = thisColor;


        //Starting Coroutines
        StartCoroutine(ShootHandler());
        StartCoroutine(HandleLightingIntensity());
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.transform.position - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle += this.angleOffset;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (health < 0.1f)
        {
            Die();
        }
    }

    private IEnumerator ShootHandler()
    {
        yield return new WaitForSeconds(3f);

        while (gameObject && player.GetComponent<PlayerShip>().GetHealth() > player.GetComponent<PlayerShip>().GetMinHealth())
        {
            //Shootings
            if (Random.Range(1, 2) == 1 && Vector2.Distance(transform.position, player.transform.position) <= 12)
            {
                isFiring = true;
                yield return new WaitForSeconds(Random.Range(0.5f,1f)); // Let Lights spool up

                Vector2 offset = this.transform.up * 1f; // 'up' is relative to the ship's rotation

                GameObject projectile = Instantiate(projectilePrefab, (Vector2) this.transform.position + offset, this.transform.localRotation); // Offset so it's not inside the enemy
                projectile.GetComponent<Projectile>().SetDamage(GetDamageFromDifficulty());
                projectile.gameObject.transform.SetParent(transform);

                //Change Projectile Color to match lights
                projectile.GetComponent<SpriteRenderer>().color = new Color (thisColor.r, thisColor.g, thisColor.b, 1f);

                StartCoroutine(ScriptUtils.PositionLerp(projectile.transform, projectile.transform.position, this.transform.up * 1000,  25.5f));
                
                
            }
            isFiring = false;
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f,2f)); // Cooldown
        }   
    }

    private IEnumerator HandleLightingIntensity()
    {
            
        while (true)
        {
            // List<Color> rightFireEnginesColors = new List<Color>();
            // List<Color> leftFireEnginesColors = new List<Color>();
            // List<Color> mainFireEnginesColors = new List<Color>();
            // List<Color> backFireEnginesColors = new List<Color>();

            // foreach (var engine in rightFireEngines)
            // {
            //     rightFireEnginesColors.Add(engine.spriteRenderer.color);
            // }
            // foreach (var engine in leftFireEngines)
            // {
            //     leftFireEnginesColors.Add(engine.spriteRenderer.color);
            // }
            // foreach (var engine in mainFireEngines)
            // {
            //     mainFireEnginesColors.Add(engine.spriteRenderer.color);
            // }
            // foreach (var engine in backFireEngines)
            // {
            //     backFireEnginesColors.Add(engine.spriteRenderer.color);
            // }


            Color lightsTempColor = lightsSpriteRenderer.color;  //We want the lights of the ship to light up more when Shooting and decrease when not
            
            // Color engineRightTempColor = ScriptUtils.GetAverageColor(rightFireEnginesColors);
            // Color engineRightHealthColor = CalculateColorBasedOnHealth(engineRightTempColor, Color.red);
            // Color engineLeftTempColor = ScriptUtils.GetAverageColor(leftFireEnginesColors);
            // Color engineLeftHealthColor = CalculateColorBasedOnHealth(engineLeftTempColor, Color.red);
            // Color engineMainTempColor = mainFireEnginesColors[0];
            // Color engineMainHealthColor = CalculateColorBasedOnHealth(engineMainTempColor , Color.red);
            // Color engineBackTempColor = ScriptUtils.GetAverageColor(backFireEnginesColors);
            // Color engineBackHealthColor = CalculateColorBasedOnHealth(engineBackTempColor, Color.red);

            // if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) // Right Engines for Moving Left
            // {
            //     engineRightTempColor.a += ScriptUtils.AddWithMax(engineRightTempColor.a, 0.01f, 1.5f);
            // }
            // else 
            // {
            //     if (engineRightTempColor.a - 0.01f >= 0)
            //     {
            //         engineRightTempColor.a -= (0.05f) / 15f ;
            //     }
            //     else 
            //     {
            //         engineRightTempColor.a = 0;
            //     }
            // }

            // if (Input.GetKey(KeyCode.RightArrow)|| Input.GetKey(KeyCode.D)) // LeftEngines for Moving Right
            // {
            //     engineLeftTempColor.a += ScriptUtils.AddWithMax(engineLeftTempColor.a, 0.01f, 1.5f);
            // }
            // else 
            // {
            //     if (engineLeftTempColor.a - 0.01f >= 0)
            //     {
            //         engineLeftTempColor.a -= (0.05f) / 15f ;
            //     }
            //     else 
            //     {
            //         engineLeftTempColor.a = 0;
            //     }
            // }

            // if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.F))// Back engines for moving forwards
            // {
            //     engineMainTempColor.a += ScriptUtils.AddWithMax(engineMainTempColor.a, 0.01f, 1.5f);
            // }
            // else 
            // {
            //     if (engineMainTempColor.a - 0.01f >= 0)
            //     {
            //         engineMainTempColor.a -= (0.05f) / 15f ;
            //     }
            //     else 
            //     {
            //         engineMainTempColor.a = 0;
            //     }
            // }

            // if (Input.GetKey(KeyCode.V) || Input.GetKey(KeyCode.J)) // Front Engines for moving Back
            // {
            //     engineBackTempColor.a += ScriptUtils.AddWithMax(engineBackTempColor.a, 0.01f, 1.5f);
            // }
            // else 
            // {
            //     if (engineBackTempColor.a - 0.01f >= 0)
            //     {
            //         engineBackTempColor.a -= (0.05f) / 15f ;
            //     }
            //     else 
            //     {
            //         engineBackTempColor.a = 0;
            //     }
            // }

            // if (Input.GetKey(KeyCode.N)|| Input.GetKey(KeyCode.L)) // Fire all Engines to slow down
            // {
            //     engineMainTempColor.a += ScriptUtils.AddWithMax(engineMainTempColor.a, 0.01f, UnityEngine.Random.Range(1.0f, 1.5f));
            //     engineRightTempColor.a += ScriptUtils.AddWithMax(engineRightTempColor.a, 0.01f, UnityEngine.Random.Range(1.0f, 1.5f));
            //     engineLeftTempColor.a += ScriptUtils.AddWithMax(engineLeftTempColor.a, 0.01f, UnityEngine.Random.Range(1.0f, 1.5f));
            //     engineBackTempColor.a += ScriptUtils.AddWithMax(engineBackTempColor.a, 0.01f, UnityEngine.Random.Range(1.0f, 1.5f));
            // }
            // else 
            // {
            //     if (engineMainTempColor.a - 0.01f >= 0)
            //     {
            //         engineMainTempColor.a -= (0.05f) / 15f ;
            //     }
            //     else 
            //     {
            //         engineMainTempColor.a = 0;
            //     }

            //      if (engineLeftTempColor.a - 0.01f >= 0)
            //     {
            //         engineLeftTempColor.a -= (0.05f) / 15f ;
            //     }
            //     else 
            //     {
            //         engineLeftTempColor.a = 0;
            //     }

            //     if (engineRightTempColor.a - 0.01f >= 0)
            //     {
            //         engineRightTempColor.a -= (0.05f) / 15f ;
            //     }
            //     else 
            //     {
            //         engineRightTempColor.a = 0;
            //     }

            //     if (engineBackTempColor.a - 0.01f >= 0)
            //     {
            //         engineBackTempColor.a -= (0.05f) / 15f ;
            //     }
            //     else 
            //     {
            //         engineBackTempColor.a = 0;
            //     }
            // }

            if (isFiring) // Fire all Engines to slow down
            {
                lightsTempColor.a += ScriptUtils.AddWithMax(lightsTempColor.a, 0.1f, 1.5f);
            }
            // else 
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

            lightsSpriteRenderer.color = lightsTempColor;
            bodyLight.color = lightsTempColor;


            // foreach (var engine in leftFireEngines)
            // {
            //     engine.spriteRenderer.color = engineLeftTempColor;
            //     engine.lightSource.color = engineLeftTempColor;
            // }
            // foreach (var engine in rightFireEngines)
            // {
            //     engine.spriteRenderer.color = engineRightTempColor;
            //     engine.lightSource.color = engineRightTempColor;
            // }
            // foreach (var engine in mainFireEngines)
            // {
            //     engine.spriteRenderer.color = engineMainTempColor;
            //     engine.lightSource.color = engineMainTempColor;
            // }
            // foreach (var engine in backFireEngines)
            // {
            //     engine.spriteRenderer.color = engineBackTempColor;
            //     engine.lightSource.color = engineBackTempColor;
            // }

            yield return null;
        }
    }

    private float GetDamageFromDifficulty()
    {
        switch (GameManager.currentDifficulty)
        {
            case GameManager.Difficulties.easy:
                return 0.11f;
                
            case GameManager.Difficulties.moderate:
                return 0.33f;

            case GameManager.Difficulties.hard:
                return 0.5f;

            default:
                return 0.2f;
        }
    }

    private void Die()
    {
        GameManager.AddToCurrentScore(5);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            Projectile projectile = other.GetComponent<Projectile>();
            if (projectile != null)
            {
                float damage = projectile.GetDamage();
                Debug.Log($"Projectile hit! Damage: {damage}");

                health -= damage;
                Debug.Log($"Enemy health: {health}");

                Destroy(other.gameObject);
            }
            else
            {
                Debug.LogWarning("Projectile script not found on object!");
            }
        }
        else
        {
            //Debug.Log($"Collision ignored with: {other.gameObject.name}");
        }
    }
}
