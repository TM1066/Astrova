using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.InputSystem.Controls;

public class GameTracker : MonoBehaviour
{
    private static GameTracker instance = null;

    // Variables to be pulled from for other scripts

    public TMP_FontAsset defaultFontAsset;
    public RuntimeAnimatorController genericButtonAnimatorController;

    public Sprite cubeUISprite;

    public AudioClip PlayerDeathSound;

    public AudioClip menuInteractSound;

    private string[] noFloatiesSceneNames = new string[4] {"Main Menu", "Name Input Screen", "End Scene", "Controls"}; // add all scene that carry on objects from the spawners shouldn't appear in  

    public InputActionReference exitAction;

    void Awake()
    {
        // Deals with duplicates

        if (instance == null) 
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object across scenes
            GameManager.SetLeaderboardFilePath(Path.Combine(Application.persistentDataPath, "leaderboard.json")); // I hate having this here it's so spaghetti 😔
            GameManager.LoadLeaderboard();

            exitAction.action.performed += ExitCheck;
            exitAction.action.Enable();
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate
        }

        // ACTUAL CODE TO BE RUN LITERALLY THE SECOND THE GAME STARTS (before first frame)

        //UnityEngine.Random.InitState(ScriptUtils.GetNumberFromString("Taylor"));

        var mouse = Mouse.current;
        mouse?.WarpCursorPosition (new Vector2 (0, 0)); //MOVE THE STUPID MOUSE OUT OF **THEEE WAYYYYY**
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
            this.GetComponent<AudioSource>().volume = 0.683f * GameManager.gameVolume;

            var audioSourcesInScene = FindObjectsByType(typeof(AudioSource), FindObjectsSortMode.None);
            foreach (AudioSource audioSource in audioSourcesInScene)
            {
                
                //audioSource.volume = 1 * GameManager.gameVolume;
            }
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

    void ExitCheck(InputAction.CallbackContext context)
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "Space Scene")
        {
            //Time.timeScale = 0f;

            //Enable Pause Menu
        }
        else if (currentSceneName == "Name Input Screen") 
        {
            SceneManager.LoadScene("Main Menu");
        }
        else if (currentSceneName == "Main Menu" || currentSceneName == "End Scene" || currentSceneName == "Space Scene")
        {
            GameManager.SaveLeaderboard();
            Application.Quit();
        }
    }
}
