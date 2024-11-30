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
    }

    public static IEnumerator ValueLerpOverTime(float startValue,float finalValue, float duration)
    {
        float timeElapsed = 0;

        while (timeElapsed < duration) 
        {
            startValue = Mathf.Lerp(startValue, finalValue, timeElapsed / duration);
            timeElapsed += Time.unscaledDeltaTime;
            yield return null;
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
