using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Shield : MonoBehaviour
{
    public float health = 1;
    public float minHealth = 0.1f;

    public Collider2D collider;
    public Slider healthBar;
    public SpriteRenderer spriteRenderer;
    public AudioSource audioSource;
    public ParticleSystem hurtParticles;
    public ParticleSystem idleParticles;

    public bool shieldActive;
    private bool shieldPoweringDown;
    private bool shieldPoweringUp;

    private Color oldColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        oldColor = spriteRenderer.color;
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

    IEnumerator DamageFlicker(float damageTaken)
    {  
        Color newColor;
        for (int i = 0; i < damageTaken * 10; i++) // should do thresholds instead of this, just for testing
        {
            newColor = ScriptUtils.GetRandomShiftedColor(spriteRenderer.color, 0.02f);
            newColor.a = Random.Range(newColor.a - 0.05f, newColor.a + 0.05f);
            StartCoroutine(ScriptUtils.ColorLerpOverTime(spriteRenderer, spriteRenderer.color, newColor, 0.1f));
            yield return new WaitForSeconds(0.1f);
        }

        if (health >= minHealth) //bring shield back up to full alpha
        {
            StartCoroutine(ScriptUtils.ColorLerpOverTime(spriteRenderer, spriteRenderer.color, GameManager.GetCurrentUserColorFullAlpha(), 0.2f));
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
