using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthSlider : MonoBehaviour
{
    private PlayerShip player;
    private float playerHealth;

    [SerializeField] UnityEngine.UI.Slider healthSlider;
    [SerializeField] ParticleSystem damageParticleSystem;

    private float fillBarRightValue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("SpaceShip").GetComponent<PlayerShip>();
        playerHealth = player.GetHealth();
        healthSlider.value = playerHealth;

        var mainParticleSystem = damageParticleSystem.main;

        mainParticleSystem.startColor = ScriptUtils.GetColorButNoAlpha(mainParticleSystem.startColor.color);

        StartCoroutine(LerpHealthSlider());
    }

    IEnumerator LerpHealthSlider()
    {
        while (true)
        {
            if (playerHealth != player.GetHealth())
            {
                if (playerHealth < player.GetHealth())
                {
                    var rotationQuat = damageParticleSystem.transform.rotation;
                    damageParticleSystem.transform.rotation = new Quaternion(rotationQuat.x, rotationQuat.y, 180, rotationQuat.w); // flip it around
                }
                else 
                {
                    var rotationQuat = damageParticleSystem.transform.rotation;
                    damageParticleSystem.transform.rotation = new Quaternion(rotationQuat.x, rotationQuat.y, 0, rotationQuat.w); // flip it back around
                }


                float startHealth = playerHealth;
                float targetHealth = player.GetHealth();
                float timeElapsed = 0f;
                float duration = 0.5f;

                var mainParticleSystem = damageParticleSystem.main;

                StartCoroutine(ScriptUtils.ColorLerpOverTime(damageParticleSystem, mainParticleSystem.startColor, ScriptUtils.GetColorButFullAlpha(mainParticleSystem.startColor), 0.2f));

                while (timeElapsed < duration)
                {
                    float t = Mathf.Clamp01(timeElapsed / duration);
                    playerHealth = Mathf.SmoothStep(startHealth, targetHealth, t);
                    healthSlider.value = playerHealth;

                    timeElapsed += Time.deltaTime;
                    yield return null;
                }

                // Ensure the final value matches exactly
                playerHealth = targetHealth;
                healthSlider.value = targetHealth;

                StartCoroutine(ScriptUtils.ColorLerpOverTime(damageParticleSystem, mainParticleSystem.startColor, ScriptUtils.GetColorButNoAlpha(mainParticleSystem.startColor), 0.2f));
            }

            yield return null;
        }
    }
}
