using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShieldHealthSlider : MonoBehaviour
{
    [SerializeField] Shield shield;
    private float shieldHealth;

    [SerializeField] UnityEngine.UI.Slider healthSlider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shieldHealth = shield.GetShieldHealth();
        healthSlider.value = shieldHealth;

        StartCoroutine(LerpHealthSlider());
    }

    IEnumerator LerpHealthSlider()
    {
        while (true)
        {
            if (shieldHealth != shield.GetShieldHealth())
            {
                float startHealth = shieldHealth;
                float targetHealth = shield.GetShieldHealth();
                float timeElapsed = 0f;
                float duration = 0.5f;

                while (timeElapsed < duration)
                {
                    float t = Mathf.Clamp01(timeElapsed / duration);
                    shieldHealth = Mathf.SmoothStep(startHealth, targetHealth, t);
                    healthSlider.value = shieldHealth;

                    timeElapsed += Time.deltaTime;
                    yield return null;
                }

                // Ensure the final value matches exactly
                shieldHealth = targetHealth;
                healthSlider.value = targetHealth;
            }

            yield return null;
        }
    }
}
