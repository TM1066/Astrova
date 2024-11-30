using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public static class GameManager
{
    private static int score = 0;

    private static string playerName;

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

    public static string GetPlayerName()
    {
        return playerName;
    }

    public static void SetPlayerName(string name)
    {
        playerName = name;
    }

    public static IEnumerator GameOver()
    {
        UiUtils.ShowMessage("GAME OVER","You Died!",UiUtils.GetCentreOfCamera(),true);

        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload Scene
    }
}
