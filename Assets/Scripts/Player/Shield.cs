using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shield : MonoBehaviour
{
    public float health = 1;
    public float minHealth = 0.1f;

    public Collider2D collider;
    public List<Image> healthBarImages; 
    public SpriteRenderer spriteRenderer;
    public AudioSource audioSource;
    public ParticleSystem hurtParticles;
    public ParticleSystem idleParticles;

    public bool shieldActive;
    private bool shieldPoweringDown;
    private bool shieldPoweringUp;

    private Color oldColor;
    private Color originColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        oldColor = spriteRenderer.color;
        originColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        //stops collision if the shield isn't visible
        if (spriteRenderer.color.a <= 0.05f)
        {
            collider.enabled = false;
        }
        else 
        {
            collider.enabled = true;
        }

        // powering up and down
        if (spriteRenderer.color.a < 1f && shieldActive && !shieldPoweringUp)
        {
            shieldPoweringUp = true;
            StartCoroutine(ScriptUtils.ColorLerpOverTime(spriteRenderer, spriteRenderer.color, ScriptUtils.GetColorButFullAlpha(spriteRenderer.color),0.4f));
        }
        else if ((spriteRenderer.color.a > 0.05f && !shieldActive && !shieldPoweringDown) | health < minHealth)
        {
            shieldPoweringDown = true;
            StartCoroutine(ScriptUtils.ColorLerpOverTime(spriteRenderer, spriteRenderer.color, ScriptUtils.GetColorButNoAlpha(spriteRenderer.color),0.5f));
        }

        if (oldColor == spriteRenderer.color)
        {
            shieldPoweringDown = false;
            shieldPoweringUp = false;
        }

        oldColor = spriteRenderer.color;

        ParticleSystem.MainModule idleParticleMain = idleParticles.main;
        idleParticleMain.startColor = spriteRenderer.color;

        foreach (var image in healthBarImages)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, spriteRenderer.color.a);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Evil Projectile"))
        {
            DecreaseHealth(Mathf.Abs(other.GetComponent<Projectile>().GetDamage()));
            Destroy(other.gameObject);
        }
    }

    void DecreaseHealth(float damage)
    {
        damage = Mathf.Abs(damage);

        if ((health - damage) > minHealth)
        {
            health -= damage;
            audioSource.Play();
        }
        else 
        {
            health = 0f;

            var colorParticleThing = hurtParticles.main;

            colorParticleThing.startColor = Color.red; // enable and make em red

            shieldActive = false;
        }
        Debug.Log("Shield Damaged for : " + damage + " Damage"); 

        health -= damage;
        StartCoroutine(DamageFlicker(damage));


    }

    public float GetShieldHealth()
    {
        return health;
    }

    IEnumerator DamageFlicker(float damageTaken)
    {  
        Color newColor;
        for (int i = 0; i < damageTaken * 10; i++) // should do thresholds instead of this, just for testing
        {
            newColor = ScriptUtils.GetRandomShiftedColor(spriteRenderer.color, 0.02f);
            newColor.a = Random.Range(newColor.a - 0.05f, newColor.a - 0.1f);
            StartCoroutine(ScriptUtils.ColorLerpOverTime(spriteRenderer, spriteRenderer.color, newColor, 0.1f));
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(ScriptUtils.ColorLerpOverTime(spriteRenderer, spriteRenderer.color, Color.clear, 0.1f));
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(ScriptUtils.ColorLerpOverTime(spriteRenderer, spriteRenderer.color, originColor, 0.3f));
            yield return new WaitForSeconds(0.1f);
        }

        if (health >= minHealth) //bring shield back up to full alpha
        {
            StartCoroutine(ScriptUtils.ColorLerpOverTime(spriteRenderer, spriteRenderer.color, originColor, 0.2f));
        }

        else //bring it down
        {

        }
        spriteRenderer.color = GameManager.GetCurrentUserColor(); // reset color
    }

    public void ActiDevate() // turning the shield on or off
    {
        if (shieldActive && !shieldPoweringUp && !shieldPoweringDown) // the rest is taken care of in Update, might move here
        {
            shieldActive = false;
        }
        else if (!shieldActive && !shieldPoweringUp && !shieldPoweringDown && health >= minHealth)
        {
            shieldActive = true;
        }
    }
}
