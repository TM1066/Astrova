using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using TMPro;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class GameTracker : MonoBehaviour
{
    private static GameTracker instance = null;

    // Variables to be pulled from for other scripts

    public TMP_FontAsset defaultFontAsset;

    //public AnimatorController uiMessageAnimator;

    public AudioClip menuInteractSound;

    void Awake()
    {
        // Deals with duplicates

        if (instance == null) 
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object across scenes
            GameManager.LoadLeaderboard();
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate
        }

        // ACTUAL CODE TO BE RUN LITERALLY THE SECOND THE GAME STARTS (before first frame)

        //UnityEngine.Random.InitState(ScriptUtils.GetNumberFromString("Taylor"));


    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GetCurrentScore() >= 100)
        {
            GameManager.currentDifficulty = GameManager.Difficulties.hard;
        }
        else if (GameManager.GetCurrentScore() >= 50)
        {
            GameManager.currentDifficulty = GameManager.Difficulties.moderate;
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q)) // Exit Checking
        {
            if (SceneManager.GetActiveScene().name == "Space Scene")
            {
                //Time.timeScale = 0f;

                //Enable Pause Menu
            }
            else if (SceneManager.GetActiveScene().name == "Name Input Screen") 
            {
                SceneManager.LoadScene("Main Menu");
            }
            else if (SceneManager.GetActiveScene().name == "Main Menu" || SceneManager.GetActiveScene().name == "End Scene")
            {
                GameManager.SaveLeaderboard();
                Application.Quit();
            }
        }
    }

    
}
