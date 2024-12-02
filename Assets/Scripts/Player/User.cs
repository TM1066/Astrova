using UnityEngine;
using System;

[System.Serializable]
public class User
{
    public string userName = "N/A";
    public Color color = Color.clear;
    public int score = -1;
    public string hexColorString;

    public string id { get; private set; } // Because weird color floating point stuff is unreliable

    public User(string name, int score, Color color)
    {
        userName = name;
        this.score = score;
        this.color = color;

        id = Guid.NewGuid().ToString();
    }

    // Convert Color to a hex string for storing in PlayFab
    public string GetColorHex()
    {
        return ColorUtility.ToHtmlStringRGB(color);
    }

    public override bool Equals(object obj)
    {
        if (obj is User otherUser)
        {
            return userName == otherUser.userName && score == otherUser.score && color .Equals(otherUser.color);
        }
        return false;
    }

    // public override int GetHashCode()
    // {
    //     return HashCode.Combine(userName, score, color); // Unreliable due to floating point arithmetic in the colors
    // }
    public override int GetHashCode()
    {
        return id.GetHashCode(); // should be nice and reliable hmmm
    }
}
