using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public static class GameManager
{
    private static int score = 0;

    private static List<User> leaderboard;  

    private static User currentUser = new User() {userName = " ", color= Color.white, score = 0};

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
        currentUser.color = color;
    }


    public static IEnumerator GameOver()
    {
        UiUtils.ShowMessage("GAME OVER","You Died!",UiUtils.GetCentreOfCamera(),true);

        yield return new WaitForSecondsRealtime(5f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload Scene
        Time.timeScale = 1f;
    }
}
