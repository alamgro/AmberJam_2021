using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Level 0
public class FishingController : MonoBehaviour
{
    private static FishingController _instance;
    public static FishingController Instance { get { return _instance; } }

    [SerializeField] private TextMeshProUGUI gameplayTimerUI;
    [SerializeField] private TextMeshProUGUI experienceUI;
    #region GAME OVER PANEL
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI expGainedUI;
    #endregion
    [SerializeField] private int timeOfGameplay;
    private int levelExperience = 0; //Exp collected 

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        GameManager.Instance.IsGameOver = false;
        gameplayTimerUI.text = "" + timeOfGameplay.ToString("00"); //Update timer on UI
        InvokeRepeating(nameof(UpdateTimer), 1f, 1f); //Start timer
    }
    
    private void UpdateTimer()
    {
        timeOfGameplay--; //Reduce time by 1
        gameplayTimerUI.text = "" + timeOfGameplay.ToString("00"); //Update timer on UI

        if(timeOfGameplay <= 0)
        {
            //Termina el minijuego
            GameManager.Instance.SaveData(); //Save level data
            GameManager.Instance.IsGameOver = true;
            GameManager.Instance.experience[0] += levelExperience; //Guardar experiencia en el nivel correcto

            //GameOver menu
            gameOverPanel.SetActive(true);
            expGainedUI.text = levelExperience.ToString("000");

            CancelInvoke(); //Stop updating timer
        }
    }

    public int LevelExperience {
        get { return levelExperience; } 

        set
        {
            levelExperience = value;
            experienceUI.text = "" + levelExperience.ToString("000");
        }
    }

}
