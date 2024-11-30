using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public static class GameManager
{
    private static int score = 0;

    private static string currentPlayerName;

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

    public static string GetCurrentPlayerName()
    {
        return currentPlayerName;
    }

    public static void SetCurrentPlayerName(string name)
    {
        currentPlayerName = name;
    }

    public static IEnumerator GameOver()
    {
        UiUtils.ShowMessage("GAME OVER","You Died!",UiUtils.GetCentreOfCamera(),true);

        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload Scene
    }
}
