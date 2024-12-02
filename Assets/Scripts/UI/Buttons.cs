using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{


    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        ScriptUtils.PlaySound(null,GameObject.Find("Game Tracker").GetComponent<GameTracker>().menuInteractSound);
    }

    public void Replay()
    {
        GameManager.leaderBoardChanged = new bool[10]{false,false,false,false,false,false,false,false,false,false}; // reset bool changed things
        SceneManager.LoadScene("Name Input Screen");
    }
}
