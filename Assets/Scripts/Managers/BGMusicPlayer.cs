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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0;
        targetVolume *= GameManager.gameVolume;
    }

    void Update()
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
