using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.Animations;
using System;
using System.Collections;

public static class UiUtils 
{

    public static IEnumerator ShowMessage(string title,string messageContents, float fontSize, Vector2 position, float lifeTime)
    {
        GameObject textGO = new GameObject("UI Message");
        textGO.transform.SetParent(GameObject.Find("Canvas").transform); // Set parent to Canvas so it'll render

        textGO.transform.position = position;

        textGO.transform.localScale = new Vector2(3f, 3f);

        textGO.AddComponent<TextMeshProUGUI>();
        textGO.GetComponent<TextMeshProUGUI>().text = messageContents; 
        textGO.GetComponent<TextMeshProUGUI>().font = GameObject.Find("Game Tracker").GetComponent<GameTracker>().defaultFontAsset; // Set Font'
        textGO.GetComponent<TextMeshProUGUI>().fontSize = fontSize;
        textGO.GetComponent<TextMeshProUGUI>().autoSizeTextContainer = true;

        textGO.GetComponent<TextMeshProUGUI>().color = Color.white;

        //ScriptUtils.ColorLerpOverTime(textGO.GetComponent<TextMeshProUGUI>(), Color.clear, Color.white,2f);

        textGO.GetComponent<RectTransform>().sizeDelta = new Vector2(1, 1);

        yield return new WaitForSecondsRealtime(lifeTime); // let it live for a while

        GameObject.Destroy(textGO);
    }

    public static Vector2 GetCentreOfCamera()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, Camera.main.nearClipPlane);

        return new Vector3 (Camera.main.ScreenToWorldPoint(screenCenter).x, Camera.main.ScreenToWorldPoint(screenCenter).y, 0);
    }

    public static Vector2 GetRandomPointInCameraBounds()
    {
        Camera mainCamera = Camera.main; // Default to main camera if not set

        // Get the camera bounds in world space
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        float minX = mainCamera.transform.position.x - cameraWidth / 2;
        float maxX = mainCamera.transform.position.x + cameraWidth / 2;
        float minY = mainCamera.transform.position.y - cameraHeight / 2;
        float maxY = mainCamera.transform.position.y + cameraHeight / 2;

        // Generate a random point within these bounds
        float randomX = UnityEngine.Random.Range(minX, maxX);
        float randomY = UnityEngine.Random.Range(minY, maxY);

        return new Vector2(randomX, randomY);
    }
}
