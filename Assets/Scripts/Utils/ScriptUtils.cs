using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;

public static class ScriptUtils
{
    public static Color GetAverageColor(List<Color> colors)
    {
        if (colors == null || colors.Count == 0)
        {
            return Color.clear; // Default to clear if no colors are provided
        }

        float r = 0f, g = 0f, b = 0f, a = 0f;

        foreach (Color color in colors)
        {
            r += color.r;
            g += color.g;
            b += color.b;
            a += color.a;
        }

        int count = colors.Count;
        return new Color(r / count, g / count, b / count, a / count);
    }

    public static Color GetRandomShiftedColor(Color baseColor, float shiftValue)
    {
        float shiftedR = UnityEngine.Random.Range(baseColor.r - shiftValue, baseColor.r + shiftValue);
        float shiftedG = UnityEngine.Random.Range(baseColor.g - shiftValue, baseColor.g + shiftValue);
        float shiftedB = UnityEngine.Random.Range(baseColor.b - shiftValue, baseColor.b + shiftValue);

        return new Color(shiftedR, shiftedG, shiftedB, baseColor.a);
    }

    public static Color GetColorButNoAlpha(Color color)
    {
        return new Color (color.r, color.g, color.b, 0f);
    }

    public static ParticleSystem.MinMaxGradient GetColorButNoAlpha(ParticleSystem.MinMaxGradient color)
    {
        Color actualColor = color.color;

        return new ParticleSystem.MinMaxGradient(new Color (actualColor.r, actualColor.g, actualColor.b, 0f));
    }

    public static Color GetColorButFullAlpha(Color color)
    {
        return new Color (color.r, color.g, color.b, 1f);
    }

    public static ParticleSystem.MinMaxGradient GetColorButFullAlpha(ParticleSystem.MinMaxGradient color)
    {
        Color actualColor = color.color;

        return new ParticleSystem.MinMaxGradient(new Color (actualColor.r, actualColor.g, actualColor.b, 1f));
    }

    public static int GetNumberFromString(string chars)
    {
        int n = 0;
        foreach (char c in chars)
        {
            if (c != ' ') // ignore empty characters
            {
                n += c; // Add ASCII value
            }
        }
        return n;
    }

    public static Color GetRandomColorFromSeed()
    {
        byte r = (Byte) UnityEngine.Random.Range(50, 255); // Make sure Colours don't get too dark to see
        byte g = (Byte) UnityEngine.Random.Range(50, 255);
        byte b = (Byte) UnityEngine.Random.Range(50, 255);

        return new Color32(r,g,b,0); // everything starts out 0 alpha
    }

    public static Color GetComplimentaryColor(Color baseColor)
    {
        float r = 1f - baseColor.r + 0.3f; // Invert the red channel & make it looks slightly brighter for prettiness
        float g = 1f - baseColor.g + 0.3f; // Invert the green channel & make it looks slightly brighter for prettiness
        float b = 1f - baseColor.b + 0.3f; // Invert the blue channel & make it looks slightly brighter for prettiness

        return new Color(r, g, b, baseColor.a); // Preserve the alpha channel
    }

    public static void PlaySound(AudioSource audioSource, AudioClip sound)
    {

        if (audioSource == null)
        {
            audioSource = GameObject.Find("Game Tracker").GetComponent<AudioSource>();
        }

        audioSource.resource = sound;
        audioSource.Play();
    }

    public static IEnumerator PositionLerp(Transform thingToMove, Vector3 vectorFrom, Vector3 vectorTo, float duration)
    {
        float timeElapsed = 0;

        while (timeElapsed < duration) 
        {
            if (thingToMove != null) // I like to destroy
            {
                thingToMove.position = Vector3.Lerp(vectorFrom, vectorTo, timeElapsed / duration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            else 
            {
                break;
            }
        }
        yield return null;
    }

    public static IEnumerator BooleanDelay(System.Func<bool> getBool, System.Action<bool> setBool, float duration) // Hell yeah
    {
        bool currentValue = getBool(); // Get the current value
        setBool(!currentValue);        // Toggle it

        yield return new WaitForSeconds(duration);

        setBool(currentValue);         // Revert to the original value
    }

    public static IEnumerator SlowTime(float howSlow, float duration)
    {
        Time.timeScale = howSlow;

        yield return new WaitForSecondsRealtime(duration);

        ValueLerpOverTime(Time.timeScale, 1.0f, 0.1f / howSlow);

        Time.timeScale = 1f;
    }

    public static IEnumerator ValueLerpOverTime(float startValue,float finalValue, float duration)
    {
        float timeElapsed = 0;

        while (timeElapsed < duration) 
        {
            startValue = Mathf.Lerp(startValue, finalValue, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
   
    public static IEnumerator SmoothValueLerpOverTime(float startValue,float finalValue, float duration)
    {
        float timeElapsed = 0;

        while (timeElapsed < duration) 
        {
            startValue = Mathf.SmoothStep(startValue, finalValue, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
    
    public static IEnumerator ColorLerpOverTime(Image image, Color startColor, Color finalColor, float duration)
    {
        if (image == null)
        {
            Debug.LogError("Image is null! Check your references.");
            yield break;
        }

        float timeElapsed = 0;

        //Debug.Log($"Starting color change from {startColor} to {finalColor} on {image.gameObject.name}");

        while (timeElapsed < duration)
        {
            image.color = Color.Lerp(startColor, finalColor, timeElapsed / duration);
            //Debug.Log($"[{image.gameObject.name}] Current color: {image.color}");
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        image.color = finalColor; // Ensure final color is set
        //Debug.Log($"[{image.gameObject.name}] Final color set: {image.color}");
    }

    public static IEnumerator ColorLerpOverTime(SpriteRenderer image, Color startColor, Color finalColor, float duration)
    {
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            image.color = Color.Lerp(startColor, finalColor, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the final color is set, in case the loop doesn't hit it exactly.
        image.color = finalColor;
    }

    public static IEnumerator ColorLerpOverTime(TextMeshProUGUI text, Color startColor, Color finalColor, float duration)
    {
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            text.color = Color.Lerp(startColor, finalColor, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the final color is set, in case the loop doesn't hit it exactly.
        text.color = finalColor;
    }

    public static IEnumerator ColorLerpOverTime(ParticleSystem particleSystem, ParticleSystem.MinMaxGradient startColor, ParticleSystem.MinMaxGradient finalColor, float duration)
    {
        float timeElapsed = 0;

        var mainParticleSystem = particleSystem.main;

        while (timeElapsed < duration)
        {
            mainParticleSystem.startColor = Color.Lerp(startColor.color, finalColor.color, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the final color is set, in case the loop doesn't hit it exactly.
        mainParticleSystem.startColor = finalColor;
    }

    public static IEnumerator DestroyGameObjectAfterTime(GameObject gameObjectToDestroy, float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);

        GameObject.Destroy(gameObjectToDestroy);
    }

    public static float AddWithMax(float floatToAddTo, float floatToAdd, float maxValue)
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

    public static float SubtractWithMin(float floatToSubtractFrom, float floatToSubtract, float minValue)
    {
        if ((floatToSubtractFrom - floatToSubtract) > minValue)
        {
            return floatToSubtract;
        }
        else 
        {
            return 0;
        }
    }

    public static void RegeneratePolygonCollider2DPoints(PolygonCollider2D polygonCollider, Sprite sprite)
    {
        for (int i = 0; i < polygonCollider.pathCount; i++) 
        {
            polygonCollider.SetPath(i, new List<Vector2>()); // Set all paths to blank
        }
        polygonCollider.pathCount = sprite.GetPhysicsShapeCount();

        List<Vector2> path = new List<Vector2>();
        for (int i = 0; i < polygonCollider.pathCount; i++) 
        {
        path.Clear();
        sprite.GetPhysicsShape(i, path);

        polygonCollider.SetPath(i, path.ToArray()); // Write Paths
        }
    }

    public static GameObject FindPlayerObject()
    {
        return GameObject.Find("SpaceShip");
    }

    public static PlayerShip FindPlayerScript()
    {
        return GameObject.Find("SpaceShip").GetComponent<PlayerShip>();
    }

}
