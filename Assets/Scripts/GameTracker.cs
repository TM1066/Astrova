using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using TMPro;
using UnityEditor.Animations;

public class GameTracker : MonoBehaviour
{
    private static GameTracker instance = null;

    // Variables to be pulled from for other scripts

    public TMP_FontAsset defaultFontAsset;

    public AnimatorController uiMessageAnimator;

    //public static User user; - Trying to keep this in GameManager

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
    }
}
