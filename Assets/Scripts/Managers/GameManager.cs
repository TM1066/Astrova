using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Unity.VisualScripting;
//using PlayFab.ProgressionModels;
using System;

public static class GameManager
{

    public enum Difficulties {easy, moderate, hard};
    static public Difficulties currentDifficulty = Difficulties.easy; // default to easy

    private static User currentUser = new User("", 0, Color.cyan); // default user

    [Header ("Keys")]
    public static KeyCode playerForwards = KeyCode.UpArrow; 
    public static KeyCode altPlayerForwards = KeyCode.W; 
    public static KeyCode playerBackwards = KeyCode.DownArrow;
    public static KeyCode altPlayerBackwards = KeyCode.S;
    public static KeyCode playerLeft = KeyCode.LeftArrow;
    public static KeyCode altPlayerLeft = KeyCode.A;
    public static KeyCode playerRight = KeyCode.RightArrow;
    public static KeyCode altPlayerRight = KeyCode.D;
    public static KeyCode playerStop = KeyCode.Space;
    public static KeyCode altPlayerStop = KeyCode.Return;
    public static KeyCode playerFire = KeyCode.F;
    public static KeyCode altPlayerFire = KeyCode.Z;
    public static KeyCode playerShield = KeyCode.Q;
    public static KeyCode altPlayerShield = KeyCode.X;


    private static bool colorfulShipsEnabled = false;
    public static List<string> shipColorTags = new List<string>(); // Special color handling

    public static bool gameMuted = false;
    public static float gameVolume = 1f; 

    // LEADERBOARD LOADING NONSENSE
    //private static string leaderboardFilePath = Path.Combine(Application.persistentDataPath, "leaderboard.json");
    private static string leaderboardFilePath; // this is defined by the gameTracker on start up, this is very dumb and i hate it, blame Unity
    public static List<User> leaderboard = new List<User>();
    public static bool[] leaderBoardChanged = new bool[10] {false,false,false,false,false,false,false,false,false,false}; // Remember to change all back to false upon replay in end scene

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

        User userToAdd = new User(userName, score, userColor);

        if (leaderboard.Count == 10 && score > leaderboard[9].score)
        {
            leaderboard.RemoveAt(9);
            leaderboard.Add(userToAdd);
        }
        else if (leaderboard.Count < 10)
        {
            leaderboard.Add(userToAdd);
        }

        // Sort leaderboard by score (descending)
        leaderboard.Sort((a, b) => b.score.CompareTo(a.score));

        if (leaderboard.IndexOf(userToAdd) >= 0) // will return -1 if the user hasn't been added
        {
            leaderBoardChanged[leaderboard.IndexOf(userToAdd)] = true;
        }

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

    public static void SetLeaderboardFilePath(string filePath)
    {
        leaderboardFilePath = filePath;
    }

    public static Color GetCurrentUserColorFullAlpha()
    {
        return new Color(currentUser.color.r, currentUser.color.g, currentUser.color.b, 1f);
    }

    public static void SetCurrentUserColor(Color color)
    {
        currentUser.hexColorString = UnityEngine.ColorUtility.ToHtmlStringRGB(color);
        currentUser.color = color;
    }


    public static bool GetColorfulShipsEnabled()
    {
        return colorfulShipsEnabled;
    }

    public static void SetColorfulShipsEnabled(bool value)
    {
        colorfulShipsEnabled = value;
    }

    public static IEnumerator GameOver()
    {
        //UiUtils.ShowMessage("GAME OVER","You Died!",UiUtils.GetCentreOfCamera(),true);

        GameObject.Destroy(GameObject.Find("Main Camera").GetComponent<CameraFollow>()); // Hopefully doesn't just delete the camera

        // for (int i = 0; i < GameObject.Find("SpaceShip").transform.childCount - 1; i++)
        // {
        //     var shipPart = GameObject.Find("SpaceShip").transform.GetChild(i);
        //     shipPart.AddComponent<Rigidbody2D>();
        //     shipPart.GetComponent<Rigidbody2D>().AddForce(GameObject.Find("SpaceShip").GetComponent<Rigidbody2D>().linearVelocity); // preserve parent speed

        //     shipPart.GetComponent<Rigidbody2D>().AddForce(new Vector2(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5))); // sprice it up a lil

        //     shipPart.GetComponent<Rigidbody2D>().angularVelocity = UnityEngine.Random.Range(0, 60);

        // }

        // GameObject.Find("SpaceShip").transform.DetachChildren(); // make the ship break up

        //Debug.Log(Time.timeScale);

        //SAVING PLAYER TO LEADERBOARD
        AddUserToLeaderboard(currentUser.userName, currentUser.score, currentUser.color);
        currentUser.score = 0;
        if (!gameMuted)
        {
            ScriptUtils.PlaySound(null, GameObject.Find("Game Tracker").GetComponent<GameTracker>().PlayerDeathSound);

            GameObject.Find("Music Player").GetComponent<AudioSource>().loop = false;

            while (GameObject.Find("Music Player").GetComponent<AudioSource>().isPlaying) // Line us up for the last scene to play the outro to the song
            {
                yield return null;
            }
        }
        else 
        {
            yield return new WaitForSecondsRealtime(4);
        }
        SceneManager.LoadScene("End Scene"); // Reload Scene
        Time.timeScale = 1f;
    }
}
