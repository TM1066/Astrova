using UnityEngine;
using UnityEngine.UI;

public class Toggles : MonoBehaviour
{
    public void SwitchColorFullShipsEnabled()
    {
        switch (GameManager.GetColorfulShipsEnabled())
        {
            case false:
                GameManager.SetColorfulShipsEnabled(true);
                this.GetComponentInChildren<Image>().color = Color.black; // use this for pretty much every trigger to change color if it's active
                break;

            case true:
                GameManager.SetColorfulShipsEnabled(false);
                this.GetComponentInChildren<Image>().color = Color.white; // use this for pretty much every trigger to change color if it's active
                break;
        }

        ScriptUtils.PlaySound(null, GameObject.Find("Game Tracker").GetComponent<GameTracker>().menuInteractSound);
    }

    public void SwitchGameMuted()
    {
        switch (GameManager.gameMuted)
        {
            case false:
                GameManager.gameMuted = true;
                this.GetComponentInChildren<Image>().color = Color.black; // use this for pretty much every trigger to change color if it's active
                break;

            case true:
                GameManager.gameMuted = false;
                this.GetComponentInChildren<Image>().color = Color.white; // use this for pretty much every trigger to change color if it's active
                break;
        }

        ScriptUtils.PlaySound(null, GameObject.Find("Game Tracker").GetComponent<GameTracker>().menuInteractSound);
    }
}
