using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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
        float r = 1f - baseColor.r + 0.2f; // Invert the red channel & make it looks slightly brighter for prettiness
        float g = 1f - baseColor.g + 0.2f; // Invert the green channel & make it looks slightly brighter for prettiness
        float b = 1f - baseColor.b + 0.2f; // Invert the blue channel & make it looks slightly brighter for prettiness

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
            timeElapsed += Time.fixedUnscaledDeltaTime;
            yield return null;
        }
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
            polygonCollider.SetPath(i, new List<Vector2>()); // Clear Paths
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
}
