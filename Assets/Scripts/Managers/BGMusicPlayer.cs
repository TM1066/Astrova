using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class BGMusicPlayer : MonoBehaviour
{

    private float timeElapsed = 0;

    private AudioSource audioSource;

    [SerializeField] float startVolume = 0.5f;
    [SerializeField] float targetVolume = 0.5f;
    [SerializeField] float startUpTime = 5f;

    [SerializeField] AudioClip CHRISTMAS;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0;
        targetVolume *= GameManager.gameVolume;

        if (GameManager.GetCurrentUserName().ToUpper() == "ELF" || (DateTime.Now.Month == 12 && DateTime.Now.Day == 25))
        {
            audioSource.clip = CHRISTMAS;
            audioSource.volume = 1;
            audioSource.Play();
        }

    }

    void Update()
    {
        if (audioSource.clip != CHRISTMAS)
        {
            if (audioSource.volume < targetVolume && !GameManager.gameMuted)
            {
                if (timeElapsed < startUpTime) 
                {
                    audioSource.volume = Mathf.Lerp(startVolume, targetVolume, timeElapsed / startUpTime);
                    timeElapsed += Time.deltaTime;
                }
            }
        }
    }
}
