using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Score:" + GameManager.GetCurrentScore().ToString();
    }

    void Update()
    {
        if (!GameObject.Find("SpaceShip").GetComponent<PlayerShip>().GetIsDead())
        {
            scoreText.text = "Score:" + GameManager.GetCurrentScore().ToString();
        }
        
    }
}
