using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuInitialiser : MonoBehaviour
{

    [SerializeField] Image colorfulShipsToggleImage;
    [SerializeField] Image mutedToggleImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        switch (GameManager.GetColorfulShipsEnabled())
        {
            case true:
                colorfulShipsToggleImage.color = Color.black;
                break;
            case false:
                colorfulShipsToggleImage.color= Color.white;
                break;
        }

        switch (GameManager.gameMuted)
        {
            case true:
                mutedToggleImage.color = Color.black;
                break;
            case false:
                mutedToggleImage.color= Color.white;
                break;
        }
    }
}
