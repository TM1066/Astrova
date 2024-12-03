using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using Unity.VisualScripting;
// using PlayFab; - SHELVED FOR NOW! - focusing on Arcane machine first and foremost
// using PlayFab.ClientModels;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] scoreTexts = new TextMeshProUGUI[10];
    [SerializeField] TextMeshProUGUI[] highScoreTexts = new TextMeshProUGUI[10];
    [SerializeField] TextMeshProUGUI[] nameTexts = new TextMeshProUGUI[10];
    [SerializeField] List<Image> colorDisplays = new List<Image>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Setting up all entries on Leaderboard
        foreach (TextMeshProUGUI scoreText in scoreTexts)
        {
            scoreText.text = "";
        }
        foreach (TextMeshProUGUI nameText in nameTexts)
        {
            nameText.text = "";
        }
        foreach (Image image in colorDisplays)
        {
            image.color = Color.clear;
        }
        foreach (TextMeshProUGUI text in highScoreTexts)
        {
            text.enabled = false;
        }

        // Sequence VERY important here

        int leaderboardAccessIndex = 0;
        
        foreach (User user in GameManager.leaderboard)
        {
            nameTexts[leaderboardAccessIndex].text = user.userName;
            colorDisplays[leaderboardAccessIndex].color = new Color(user.color.r, user.color.g, user.color.b, 1f);
            scoreTexts[leaderboardAccessIndex].text = user.score.ToString();

            leaderboardAccessIndex++;
        }
    }

    private void SetHighScoreMessages()
    {
        for (int i = 0; i < GameManager.leaderBoardChanged.Length - 1; i++)
        {
            if (GameManager.leaderBoardChanged[i])
            {
                highScoreTexts[i].enabled = true; // re-enable high score texts for the places on the board that it should be shown
            }
        }
    }

    public void SetAllColours()
    {

        Animator animator = GameObject.Find("LeaderBoard").GetComponent<Animator>();
            if (animator != null)
            {
                animator.enabled = false; // Disable animation for the object
            }

        // for (int i = 0; i < colorDisplays.Count; i++)
        // {

        //     Debug.Log("Changing ColorDisplay " + i);
        //     Debug.Log("current color: " + colorDisplays[i].color);

        //     //colorDisplays[i].color = GameManager.leaderboard[i].color;
        //     colorDisplays[i].color = Color.red;

        //     Debug.Log("changed color: " + colorDisplays[i].color);
        // }

        StartCoroutine(ChangeAllColorsProperly());
    }
    
    private IEnumerator ChangeAllColorsProperly()
    {
        for (int i = 0; i < colorDisplays.Count - 1; i++)
        {
            Image currentColorImage = colorDisplays[i];

            Color colorToChangeTo = GameManager.leaderboard[i].color;
            colorToChangeTo.a = 1f;

            StartCoroutine(ScriptUtils.ColorLerpOverTime(currentColorImage, currentColorImage.color, colorToChangeTo, 0.5f));
            yield return new WaitForSeconds(0.2f);
        }
    }
}

    // Fetch the leaderboard from PlayFab, ordered by score
    // public void GetLeaderboard()
    // {
    //     var request = new GetLeaderboardRequest
    //     {
    //         StatisticName = "score",  // The name of the statistic to order by
    //         StartPosition = 0,        // Start at the top
    //         MaxResultsCount = 10      // Number of results to fetch (top 10)
    //     };

    //     PlayFabClientAPI.GetLeaderboard(request, result => {
    //         Debug.Log("Leaderboard retrieved successfully.");

    //         // Iterate over the leaderboard entries and log them
    //         foreach (var entry in result.Leaderboard)
    //         {
    //             Debug.Log($"Player: {entry.DisplayName}, Score: {entry.StatValue}");
    //             // Here, you can populate your UI with the retrieved leaderboard
    //         }
    //     }, error => {
    //         Debug.LogError("Error retrieving leaderboard: " + error.ErrorMessage);
    //     });
    // }
