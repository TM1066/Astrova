using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;

public class NameInput : MonoBehaviour
{
    public GameObject[] charInputFields; // should be gameobjects that 

    public Dictionary<GameObject,char> charInputValueDictionary = new Dictionary<GameObject,char>();

    private AudioSource audioSource; // Audio Player
    [SerializeField] AudioClip charChangeAudio;
    [SerializeField] AudioClip charConfirmAudio;

    private float charScrollSpeed = 0.2f;

    private string[] illegalNames = new string[2] { "fag","kkk"};

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        foreach (GameObject gameObjectcharInputField in charInputFields)
        {
            //gameObjectcharInputField.GetComponent<Button>().onClick.AddListener(MoveToNextInputField); // Only works with enter for some reasonsss

            // Population Dictionary with ' '
            charInputValueDictionary.Add(gameObjectcharInputField, ' ');

            gameObjectcharInputField.GetComponentInChildren<TextMeshProUGUI>().text = charInputValueDictionary[gameObjectcharInputField].ToString();

            StartCoroutine(CharSelectorListener(gameObjectcharInputField)); // Handling Char changing
        }
        // StartCoroutine(UIHandlingStuff());
    }

    void MoveToPreviousInputField()
    {
        var currentlySelectedField = EventSystem.current.currentSelectedGameObject;

        if (currentlySelectedField != null)
        {
            var button = currentlySelectedField.GetComponent<Button>();
            if (button != null && button.navigation.selectOnLeft != null)
            {
                EventSystem.current.SetSelectedGameObject(button.navigation.selectOnLeft.gameObject);
                PlayCharChangeSound();
            }
            else
            {
                Debug.LogWarning("No valid navigation target to the left.");
                SceneManager.LoadScene("Main Menu");
            }
        }
        else
        {
            Debug.LogError("No currently selected field.");
        }
        Debug.Log($"Currently selected: {EventSystem.current.currentSelectedGameObject?.name}");
    }

    void MoveToNextInputField()
    {
        var currentlySelectedField = EventSystem.current.currentSelectedGameObject;

        if (currentlySelectedField != null)
        {
            var button = currentlySelectedField.GetComponent<Button>();
            if (button != null && button.navigation.selectOnRight != null)
            {
                EventSystem.current.SetSelectedGameObject(button.navigation.selectOnRight.gameObject);
                PlayCharConfirmSound();
            }
            else
            {
                Debug.LogWarning("No valid navigation target to the right.");
            }
        }
        else
        {
            Debug.LogError("No currently selected field.");
        }
        Debug.Log($"Currently selected: {EventSystem.current.currentSelectedGameObject?.name}");
    }

    void PlayCharChangeSound()
    {
        ScriptUtils.PlaySound(audioSource, charChangeAudio);
    }

    void PlayCharConfirmSound()
    {
        ScriptUtils.PlaySound(audioSource, charConfirmAudio);
    }

    private IEnumerator CharSelectorListener(GameObject charInputField) // Checks for input & changes currently selected char
    {
        yield return new WaitForSecondsRealtime(0.2f); // Leave a delay so the Player can move back up to the fields from the done button

        while (this.gameObject != null && this.gameObject.activeInHierarchy)
        {
            if (EventSystem.current.currentSelectedGameObject == charInputField)
            {
                if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && charInputValueDictionary[charInputField] <= 89)
                {
                    if (charScrollSpeed > 0.08f)
                    {
                        charScrollSpeed -= 0.01f;
                    }
                    //SKIPPING ANNOYING CHARS
                    if (charInputValueDictionary[charInputField] < (char)49) // A to 9
                    {
                        charInputValueDictionary[charInputField] = (char)49;
                    }
                    else if (charInputValueDictionary[charInputField] == (char)57) // 9 to A
                    {
                        charInputValueDictionary[charInputField] = (char)65;
                    }
                    // else if (charInputValueDictionary[charInputField] == (char)90) // Z to a
                    // {
                    //     charInputValueDictionary[charInputField] = (char)97;
                    // }
                    else 
                    {
                        charInputValueDictionary[charInputField]++; // shoullddd take the ascii integer of the char, increment it and then spit another char back out
                    }

                    PlayCharChangeSound();

                    charInputField.GetComponentInChildren<TextMeshProUGUI>().text = charInputValueDictionary[charInputField].ToString();
                    yield return new WaitForSecondsRealtime(charScrollSpeed);
                }
                else if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && charInputValueDictionary[charInputField] >= 49)
                {
                    if (charScrollSpeed > 0.05f)
                    {
                        charScrollSpeed -= 0.01f;
                    }
                    //SKIPPING ANNOYING CHARS
                    if (charInputValueDictionary[charInputField] == (char)65) // A to 9
                    {
                        charInputValueDictionary[charInputField] = (char)57;
                    }
                    else if (charInputValueDictionary[charInputField] == (char)65) // A to 9
                    {
                        charInputValueDictionary[charInputField] = (char)57;
                    }
                    else if (charInputValueDictionary[charInputField] == (char)97) // a to Z
                    {
                        charInputValueDictionary[charInputField] = (char)90;
                    }
                    else 
                    {
                        charInputValueDictionary[charInputField]--; // shoullddd take the ascii integer of the char, increment it and then spit another char back out
                    }

                    PlayCharChangeSound();

                    charInputField.GetComponentInChildren<TextMeshProUGUI>().text = charInputValueDictionary[charInputField].ToString();
                    yield return new WaitForSecondsRealtime(charScrollSpeed);
                }
                else 
                {
                    charScrollSpeed = 0.2f;
                }
                
                if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.F))
                {
                    yield return new WaitForEndOfFrame(); // Ensure no conflicts with internal navigation
                    MoveToNextInputField();
                }
                else if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.G))
                {
                    yield return new WaitForEndOfFrame(); // Ensure no conflicts with internal navigation
                    MoveToPreviousInputField();
                }
            }
            yield return null;
        }
    }

    public void DoneButton()
    {
        PlayCharConfirmSound();

        //Set random Seed to Name & Close all UI + reset Time Scale
        string seedString = "";

        foreach (GameObject charInputField in charInputFields)
        {
            seedString += charInputValueDictionary[charInputField];
        }

        seedString = seedString.Trim(' ');

        if (seedString == "" || seedString == "   " || seedString == " " || seedString == "  " || illegalNames.Contains(seedString.ToLower()))
        {
            seedString = "N/A";
        }

        Debug.Log($"Random seed will be generated from string: {seedString}");
        GameManager.SetCurrentUserName(seedString);
        UnityEngine.Random.InitState(ScriptUtils.GetNumberFromString(seedString));
        GameManager.SetCurrentUserColor(ScriptUtils.GetRandomColorFromSeed());


        // SPECIAL NAMES
        if (seedString.ToLower() == "tm")
        {
            GameManager.SetCurrentUserColor(new Color (0.56769f, 0.709538f, 0.745f));
        }
        else if (seedString.ToLower() == "god")
        {
            GameManager.SetCurrentUserColor(new Color (112,128,144));
        }
        else if (seedString.ToLower() == "666")
        {
            GameManager.SetCurrentUserColor(new Color (0,0,0));
        }
        else if (seedString.ToLower() == "xiv")
        {
            GameManager.SetCurrentUserColor(new Color (1,0.504717f,0.8643153f));
        }
        else if (seedString.ToLower() == "luk")
        {
            GameManager.SetCurrentUserColor(new Color (0.08627451f,0.1411765f,0.5019608f));
        }
        else if (seedString.ToLower() == "jam")
        {
            GameManager.SetCurrentUserColor(new Color (2560000f,0f,0f));
        }
        else if (seedString.ToLower() == "dor")
        {
            GameManager.SetCurrentUserColor(new Color (0f,1f,0f));
        }
        else if (seedString.ToLower() == "582")
        {
            GameManager.SetCurrentUserColor(new Color (0.01568628f,0.4470588f,0.7803922f));
        }
        else if (seedString.ToLower() == "nhm")
        {
            GameManager.SetCurrentUserColor(new Color (0.6196079f,0.05490196f,0.06666667f));
        }
        else if (seedString.ToLower() == "amy")
        {
            GameManager.SetCurrentUserColor(new Color (1,0.5990566f,0.9183913f));
        }
        else if (seedString.ToLower() == "ele")
        {
            GameManager.SetCurrentUserColor(new Color (1,0.5990566f,0.9183913f));
        }
        else if (seedString.ToLower() == "ek")
        {
            GameManager.SetCurrentUserColor(new Color (1,0.5990566f,0.9183913f));
        }
        else if (seedString.ToLower() == "spk")
        {
            GameManager.SetCurrentUserColor(new Color (0f,0f,0f));
            GameManager.shipColorTags.Add("spk");
        }
        else if (seedString.ToLower() == "elf")
        {
            GameManager.SetCurrentUserColor(new Color (0f,0f,0f));
            GameManager.shipColorTags.Add("elf");
        }
        else if (seedString.ToLower() == "css")
        {
            GameManager.SetCurrentUserColor(new Color (0.2941177f,0.2078431f,0.5411765f));
        }

        Time.timeScale = 1.0f;

        GameObject.Destroy(this.gameObject);

        SceneManager.LoadScene("Space Scene");
    }
}
