using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using TMPro;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using System.Linq;
using Unity.VisualScripting;

public class GameTracker : MonoBehaviour
{
    private static GameTracker instance = null;

    // Variables to be pulled from for other scripts

    public TMP_FontAsset defaultFontAsset;
    public RuntimeAnimatorController genericButtonAnimatorController;

    public Sprite cubeUISprite;

    public AudioClip PlayerDeathSound;

    //public AnimatorController uiMessageAnimator;

    public AudioClip menuInteractSound;

    private string[] noFloatiesSceneNames = new string[4] {"Main Menu", "Name Input Screen", "End Scene", "Controls"}; // add all scene that carry on objects from the spawners shouldn't appear in  

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

          var mouse = Mouse.current;

          mouse.WarpCursorPosition (new Vector2 (0, 0)); //MOVE THE STUPID MOUSE OUT OF **THEEE WAYYYYY**
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
        else 
        {
            GameManager.currentDifficulty = GameManager.Difficulties.easy;
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
            else if (SceneManager.GetActiveScene().name == "Main Menu" || SceneManager.GetActiveScene().name == "End Scene" || SceneManager.GetActiveScene().name == "Space Scene")
            {
                GameManager.SaveLeaderboard();
                Application.Quit();
            }
        }
    

        if (GameManager.gameMuted) // mute scene if game is muted -- definitely a better way of doing this
        {
            var audioSourcesInScene = FindObjectsByType(typeof(AudioSource), FindObjectsSortMode.None);

            foreach (AudioSource audioSource in audioSourcesInScene)
            {
                audioSource.volume = 0;
            }
        }
        else 
        {
            this.GetComponent<AudioSource>().volume = 0.683f;
        }
        // GET RID OF EVIL NO GOOD GAME OBJECTS THAT WON'T GET DELETED PROPERLY (baddddd way to do this)
        
        

        if (noFloatiesSceneNames.Contains(SceneManager.GetActiveScene().name))
        {
            foreach (var asteroid in GameObject.FindGameObjectsWithTag("Asteroid"))
            {
                Destroy(asteroid);
            }

            foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                Destroy(enemy);
            }

            foreach (var item in GameObject.FindGameObjectsWithTag("Item"))
            {
                Destroy(item);
            }

            // foreach (var star in GameObject.FindGameObjectsWithTag("Star"))
            // {
            //     Destroy(star);
            // }
        }
    }
}
