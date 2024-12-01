using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
// using PlayFab; - SHELVED FOR NOW! - focusing on Arcane machine first and foremost
// using PlayFab.ClientModels;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] List<TextMeshProUGUI> scoreTexts = new List<TextMeshProUGUI>();
    [SerializeField] List<TextMeshProUGUI> nameTexts = new List<TextMeshProUGUI>();
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

        // Sequence VERY important here

        int leaderboardAccessIndex = 0;
        foreach (User user in GameManager.leaderboard)
        {
            Debug.Log($"Name: {user.userName}, Score: {user.score}, Color: {user.color}");

            nameTexts[leaderboardAccessIndex].text = user.userName;
            colorDisplays[leaderboardAccessIndex].color = new Color(user.color.r, user.color.g, user.color.b, 1f);
            scoreTexts[leaderboardAccessIndex].text = user.score.ToString();

            leaderboardAccessIndex++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
