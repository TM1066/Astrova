using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class GameTracker : MonoBehaviour
{
    private static GameTracker instance = null;


    

    void Awake()
    {
        // Deals with duplicates

        if (instance == null) 
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate
        }

        // ACTUAL CODE TO BE RUN LITERALLY THE SECOND THE GAME STARTS (before first frame)

        //Will come from user input soon
        UnityEngine.Random.InitState(ScriptUtils.GetNumberFromString("Taylor   "));

    }

    // Update is called once per frame
    void Update()
    {
    }
}
