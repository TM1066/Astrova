using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.Animations;
using System;

public static class UiUtils 
{

    public static void ShowMessage(string title,string messageContents, Vector2 position, bool? backGroundPanel)
    {
        GameObject textGO = new GameObject("UI Message");
        textGO.transform.SetParent(GameObject.Find("Canvas").transform); // Set parent to Canvas so it'll render

        textGO.transform.position = position;

        textGO.transform.localScale = new Vector2(3f, 3f);

        textGO.AddComponent<TextMeshProUGUI>();
        textGO.GetComponent<TextMeshProUGUI>().text = messageContents; 
        textGO.GetComponent<TextMeshProUGUI>().font = GameObject.Find("Game Tracker").GetComponent<GameTracker>().defaultFontAsset; // Set Font'

        textGO.GetComponent<TextMeshProUGUI>().color = Color.clear;

        ScriptUtils.ColorLerpOverTime(textGO.GetComponent<TextMeshProUGUI>(), Color.clear, Color.white,2f);

        textGO.AddComponent<Animator>();



        if (backGroundPanel != null)
        {
            if ((bool) backGroundPanel)
            {
                GameObject panelGO;
                panelGO = new GameObject("UI Message Panel");

                panelGO.AddComponent<RectTransform>();

                panelGO.transform.SetParent(GameObject.Find("Canvas").transform,true);

                panelGO.AddComponent<SpriteRenderer>();
                panelGO.GetComponent<SpriteRenderer>().sprite = GameObject.Find("Game Tracker").GetComponent<GameTracker>().cubeUISprite; // Load BG Panel Sprite

                panelGO.transform.position = position;

                panelGO.GetComponent<RectTransform>().sizeDelta = new Vector2(566.6f, 243.12f);

                panelGO.transform.localScale = new Vector2 (2,2);

                textGO.gameObject.transform.SetParent(panelGO.transform,true);
            }
        }
    }

        // public static void ShowMessage(string title,string messageContents, Vector2 position, bool? backGroundPanel, float secondsToExist)
        // {
        //     GameObject textGO = new GameObject("UI Message");
        //     textGO.transform.SetParent(GameObject.Find("Canvas").transform); // Set parent to Canvas so it'll render

        //     textGO.transform.position = position;

        //     textGO.transform.localScale = new Vector2(3f, 3f);

        //     textGO.AddComponent<TextMeshProUGUI>();
        //     textGO.GetComponent<TextMeshProUGUI>().text = messageContents; 
        //     textGO.GetComponent<TextMeshProUGUI>().font = GameObject.Find("Game Tracker").GetComponent<GameTracker>().defaultFontAsset; // Set Font'

        //     textGO.AddComponent<Animator>();

        //     if (backGroundPanel != null)
        //     {
        //         if ((bool) backGroundPanel)
        //         {
        //             GameObject panelGO;
        //             panelGO = new GameObject("UI Message Panel");

        //             panelGO.transform.SetParent(GameObject.Find("Canvas").transform,true);

        //             panelGO.AddComponent<SpriteRenderer>();
        //             panelGO.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/Visual/Resources/Sprites/UI/Cube UI Sprite.png"); // Load BG Panel Sprite

        //             panelGO.transform.position = position;

        //             panelGO.transform.localScale = new Vector2 (2,2);

        //             textGO.gameObject.transform.SetParent(panelGO.transform,true);
        //         }
        //     }
        // }

    public static Vector2 GetCentreOfCamera()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, Camera.main.nearClipPlane);

        return new Vector3 (Camera.main.ScreenToWorldPoint(screenCenter).x, Camera.main.ScreenToWorldPoint(screenCenter).y, 0);
    }
}
