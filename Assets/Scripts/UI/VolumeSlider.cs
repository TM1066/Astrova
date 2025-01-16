using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{

    private Slider slider;

    private float tempValue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = GameManager.gameVolume;
    }

    public void HandleVolumeSlideChange()
    {
        GameManager.gameVolume = slider.value;
        Debug.Log("Game Volume: " + GameManager.gameVolume);
        Debug.Log("Game Muted: " + GameManager.gameMuted);


        if (GameManager.gameVolume <= 0.1f)
        {
            GameManager.gameMuted = true;
            GameObject.Find("Mute Mode").GetComponentInChildren<Image>().color = Color.black;
        }
        else
        {
            GameManager.gameMuted = false;
            GameObject.Find("Mute Mode").GetComponentInChildren<Image>().color = Color.white;
            GameManager.gameVolume = slider.value;
        }
    }

    public void PlayValueChangeSound()
    {
        ScriptUtils.PlaySound(null, GameObject.Find("Game Tracker").GetComponent<GameTracker>().menuInteractSound);
    }
}
