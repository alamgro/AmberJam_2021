using UnityEngine.SceneManagement;
using UnityEngine;

public class GameScenes : MonoBehaviour
{
    public void PlayMinigame(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }

    public void GoToMainGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
