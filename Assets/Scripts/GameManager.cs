using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public static class GameManager
{
    private static int score = 0;

    private static User currentUser = new User(" ", 0, Color.white);


    // LEADERBOARD LOADING NONSENSE
    private static string leaderboardFilePath = Path.Combine(Application.persistentDataPath, "leaderboard.json");
    public static List<User> leaderboard = new List<User>();

    public static void LoadLeaderboard()
    {
        if (File.Exists(leaderboardFilePath))
        {
            string json = File.ReadAllText(leaderboardFilePath);

            LeaderboardWrapper wrapper = JsonUtility.FromJson<LeaderboardWrapper>(json);
            leaderboard = wrapper.users;
            Debug.Log("Leaderboard loaded!");
        }
        else
        {
            Debug.Log("No leaderboard file found; starting fresh.");
        }
    }
    public static void SaveLeaderboard()
    {
        string json = JsonUtility.ToJson(new LeaderboardWrapper(leaderboard), true);
        File.WriteAllText(leaderboardFilePath, json);
        Debug.Log("Leaderboard saved to: " + leaderboardFilePath);
    }

    public static void AddUserToLeaderboard(string userName, int score, Color userColor)
    {
        User newUser = new User(userName, score, userColor);
        leaderboard.Add(newUser);

        // Sort leaderboard by score (descending)
        leaderboard.Sort((a, b) => b.score.CompareTo(a.score));

        SaveLeaderboard();
    }
    // END OF LEADERBOARD NONSENSE

    public static int GetScore()
    {
        return score;
    }
    public static void SetScore(int setScore)
    {
        score = setScore;
    }
    public static void IncrementScore()
    {
        score ++;
    }
    public static void AddToScore(int toAdd)
    {
        score += toAdd;
    }

    public static string GetCurrentUserName()
    {
        return currentUser.userName;
    }

    public static void SetCurrentUserName(string name)
    {
        currentUser.userName = name;
    }

    public static Color GetCurrentUserColor()
    {
        
        return currentUser.color;
    }

    public static Color GetCurrentUserColorFullAlpha()
    {
        return new Color(currentUser.color.r, currentUser.color.g, currentUser.color.b, 1f);
    }

    public static void SetCurrentUserColor(Color color)
    {
        currentUser.hexColorString = ColorUtility.ToHtmlStringRGB(color);
        currentUser.color = color;
    }


    public static IEnumerator GameOver()
    {
        UiUtils.ShowMessage("GAME OVER","You Died!",UiUtils.GetCentreOfCamera(),true);

        //SAVING PLAYER TO LEADERBOARD
        AddUserToLeaderboard(currentUser.userName, currentUser.score, currentUser.color);

        yield return new WaitForSecondsRealtime(5f);

        SceneManager.LoadScene("End Screen"); // Reload Scene
        Time.timeScale = 1f;
    }
}
