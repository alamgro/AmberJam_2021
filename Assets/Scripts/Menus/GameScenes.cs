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

    public void PauseGame()
    {
        Time.timeScale = 0.0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
