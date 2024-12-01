using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;
using UnityEditor.Animations;

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

        textGO.AddComponent<Animator>();

        if (backGroundPanel != null)
        {
            if ((bool) backGroundPanel)
            {
                GameObject panelGO;
                panelGO = new GameObject("UI Message Panel");

                panelGO.transform.SetParent(GameObject.Find("Canvas").transform,true);

                panelGO.AddComponent<SpriteRenderer>();
                panelGO.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/Visual/Resources/Sprites/UI/Cube UI Sprite.png"); // Load BG Panel Sprite

                panelGO.transform.position = position;

                panelGO.transform.localScale = new Vector2 (2,2);

                textGO.gameObject.transform.SetParent(panelGO.transform,true);
            }
        }
    }

    public static Vector2 GetCentreOfCamera()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, Camera.main.nearClipPlane);

        return new Vector3 (Camera.main.ScreenToWorldPoint(screenCenter).x, Camera.main.ScreenToWorldPoint(screenCenter).y, 0);
    }
}
