using UnityEngine;

[System.Serializable]
public class User
{
    public string userName = "N/A";
    public Color color = Color.clear;
    public int score = -1;
    public string hexColorString;

    public User(string name, int score, Color color)
    {
        this.userName = name;
        this.score = score;
        this.color = color;
    }

    // Convert Color to a hex string for storing in PlayFab
    public string GetColorHex()
    {
        return ColorUtility.ToHtmlStringRGB(color);
    }
}
