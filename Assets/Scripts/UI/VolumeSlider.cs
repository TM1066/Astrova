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
    }

    // Update is called once per frame
    void Update()
    {
        GameManager.gameVolume = slider.value;

        if (slider.value == 0f)
        {
            GameObject.Find("Mute Mode").GetComponentInChildren<Toggle>().isOn = true;
            GameManager.gameMuted = true;
        }
        else if (slider.value > 0f && tempValue == 0f)
        {
            GameObject.Find("Mute Mode").GetComponentInChildren<Toggle>().isOn = false;
            GameManager.gameMuted = false;
        }


        tempValue = slider.value;
    }
}
