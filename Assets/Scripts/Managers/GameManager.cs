using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public static class GameManager
{

    public enum Difficulties {easy, moderate, hard};
    static public Difficulties currentDifficulty = Difficulties.easy;

    private static User currentUser = new User("", 0, Color.cyan);

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
            SaveLeaderboard();
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
        // check if the Player's score is higher than the lowest score on leaderboard / the lowest entry is empty

        if (leaderboard.Count == 10 && score > leaderboard[9].score)
        {
            leaderboard.RemoveAt(9);
            leaderboard.Add(new User(userName, score, userColor));
        }
        else if (leaderboard.Count < 10)
        {
            leaderboard.Add(new User(userName, score, userColor));
        }

        // Sort leaderboard by score (descending)
        leaderboard.Sort((a, b) => b.score.CompareTo(a.score));

        SaveLeaderboard();
    }
    // END OF LEADERBOARD NONSENSE

    public static int GetCurrentScore()
    {
        return currentUser.score;
    }
    public static void SetCurrentScore(int setScore)
    {
        currentUser.score = setScore;
    }
    public static void IncrementCurrentScore()
    {
        currentUser.score ++;
    }
    public static void AddToCurrentScore(int toAdd)
    {
        currentUser.score += toAdd;
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

        currentUser.userName = "";
        currentUser.score = 0;
        currentUser.color = Color.clear;

        yield return new WaitForSecondsRealtime(5f);

        SceneManager.LoadScene("End Scene"); // Reload Scene
        Time.timeScale = 1f;
    }
}
