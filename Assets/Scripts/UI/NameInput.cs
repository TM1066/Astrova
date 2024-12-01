using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class NameInput : MonoBehaviour
{

    public GameObject[] charInputFields; // should be gameobjects that 

    public Dictionary<GameObject,char> charInputValueDictionary = new Dictionary<GameObject,char>();

    private AudioSource audioSource; // Audio Player
    [SerializeField] AudioClip charChangeAudio;
    [SerializeField] AudioClip charConfirmAudio;

    private float charScrollSpeed = 0.2f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        foreach (GameObject gameObjectcharInputField in charInputFields)
        {
            gameObjectcharInputField.GetComponent<Button>().onClick.AddListener(MoveToNextInputField); // Should hopefully work with

            // Population Dictionary with ' '
            charInputValueDictionary.Add(gameObjectcharInputField, ' ');

            gameObjectcharInputField.GetComponentInChildren<TextMeshProUGUI>().text = charInputValueDictionary[gameObjectcharInputField].ToString();

            StartCoroutine(CharSelectorListener(gameObjectcharInputField)); // Handling Char changing
        }
        StartCoroutine(UIHandlingStuff());
    }

    void MoveToNextInputField()
    {
        var currentlySelectedField = EventSystem.current.currentSelectedGameObject;

        if (Array.IndexOf(charInputFields, currentlySelectedField) < 6) // check it's not the last input field
        {
            currentlySelectedField = charInputFields[Array.IndexOf(charInputFields, currentlySelectedField) + 1]; // grab next input field

            EventSystem.current.SetSelectedGameObject(currentlySelectedField);

            PlayCharConfirmSound();
        }

        else 
        { // Selection on last char input field
            EventSystem.current.SetSelectedGameObject(this.gameObject.transform.GetChild(0).transform.GetChild(2).gameObject); // Honestly not sure if this will work and even if it does, it'll break the second we change the amount of children!!! <- gross
            PlayCharConfirmSound();
        }
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
                if (Input.GetKey(KeyCode.UpArrow) && charInputValueDictionary[charInputField] <= 122)
                {
                    //SKIPPING ANNOYING CHARS
                    if (charInputValueDictionary[charInputField] < (char)49) // A to 9
                    {
                        charInputValueDictionary[charInputField] = (char)49;
                    }
                    else if (charInputValueDictionary[charInputField] == (char)57) // 9 to A
                    {
                        charInputValueDictionary[charInputField] = (char)65;
                    }
                    else if (charInputValueDictionary[charInputField] == (char)90) // Z to a
                    {
                        charInputValueDictionary[charInputField] = (char)97;
                    }
                    else 
                    {
                        charInputValueDictionary[charInputField]++; // shoullddd take the ascii integer of the char, increment it and then spit another char back out
                    }

                    PlayCharChangeSound();

                    charInputField.GetComponentInChildren<TextMeshProUGUI>().text = charInputValueDictionary[charInputField].ToString();
                    yield return new WaitForSecondsRealtime(charScrollSpeed);
                }
                else if (Input.GetKey(KeyCode.DownArrow) && charInputValueDictionary[charInputField] >= 49)
                {
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

        if (seedString == "")
        {
            seedString = "Anonymous";
        }

        Debug.Log($"Random seed will be generated from string: {seedString}");
        GameManager.SetCurrentUserName(seedString);
        UnityEngine.Random.InitState(ScriptUtils.GetNumberFromString(seedString));
        GameManager.SetCurrentUserColor(ScriptUtils.GetRandomColorFromSeed());

        Time.timeScale = 1.0f;

        GameObject.Destroy(this.gameObject);

        SceneManager.LoadScene("Space Scene");
    }

    IEnumerator UIHandlingStuff()
    {
        while (this.gameObject) // Will be deleted by final button along with everything else
        {
            

            yield return new WaitForSecondsRealtime(2); // Time Scale will be set to 0 while setting up name
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
