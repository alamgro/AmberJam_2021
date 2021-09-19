using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WhackamoleControler : MonoBehaviour
{
    #region SINGLETON
    private static WhackamoleControler _instance;
    public static WhackamoleControler Instance { get { return _instance; } }
    #endregion

    [SerializeField] private List<Mole> moles; //List of moles
    [SerializeField] private int maxMolesOutside; //Maximum number of moles outside at the same time
    [SerializeField] private float timeOutside; //Time that the moles stay outside
    [SerializeField] private float delayBetweenMoles; //Time between batches of moles
    [SerializeField] private int timeOfGameplay; //Minigame duration

    [SerializeField] private TextMeshProUGUI gameplayTimerUI;
    [SerializeField] private TextMeshProUGUI experienceUI;
    #region GAME OVER PANEL
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI expGainedUI;
    #endregion
    private int levelExperience = 0; //Exp collected 
    private int molesOutside = 0; //Moles currently outside 

    private void Awake()
    {
        _instance = this;   
    }

    IEnumerator Start()
    {
        GameManager.Instance.IsGameOver = false;
        gameplayTimerUI.text = "" + timeOfGameplay.ToString("00"); //Update timer on UI
        InvokeRepeating(nameof(UpdateTimer), 1f, 1f); //Start timer
        yield return new WaitForSecondsRealtime(2f);
        ChooseRandomMoles();
    }

    private void UpdateTimer()
    {
        timeOfGameplay--; //Reduce time by 1
        gameplayTimerUI.text = "" + timeOfGameplay.ToString("00"); //Update timer on UI

        if (timeOfGameplay <= 0)
        {
            //Termina el minijuego
            GameManager.Instance.SaveData(); //Save level data
            GameManager.Instance.IsGameOver = true;
            GameManager.Instance.experience[1] += levelExperience; //Guardar experiencia en el nivel correcto

            //GameOver menu
            gameOverPanel.SetActive(true);
            expGainedUI.text = levelExperience.ToString("000");

            CancelInvoke(); //Stop updating timer
        }

        CheckMolesTimeOutside();
    }

    void CheckMolesTimeOutside()
    {
        //Reduce the time that moles stay outside every 15 seconds
        if (timeOfGameplay % 15 == 0)
            timeOutside -= 0.45f;

        if (timeOutside < 0.8f)
            timeOutside = 0.8f;
    }

    public void ChooseRandomMoles()
    {
        if (GameManager.Instance.IsGameOver)
           return;

        //To spawn from 1 up to maxMolesOutside
        int amountMoles = Random.Range(1, maxMolesOutside + 1);
        //print("Salen: " + amountMoles);

        //Set up temp lists
        List<Mole> copyMolesList = new List<Mole>();
        copyMolesList.AddRange(moles.ToArray());
        List<Mole> tempList = new List<Mole>();
        
        //Chose rand mole and make it go out (Pikaboo)
        for (int i = 0; i < amountMoles; i++)
        {
            //Choose random mole from list
            int randIndex = Random.Range(0, copyMolesList.Count);
            tempList.Add(copyMolesList[randIndex]); //Add the rand item
            copyMolesList.RemoveAt(randIndex); //Remove that posible mole to avoid repeting it
            //print("Index elegido: " + randIndex);
            StartCoroutine(moles[randIndex].IEPikaboo(timeOutside));
        }
    }

    public int LevelExperience
    {
        get { return levelExperience; }

        set
        {
            levelExperience = value;
            experienceUI.text = "" + levelExperience.ToString("000");
        }
    }

    public int MolesOutside {
        get { return molesOutside; } 
        set
        {
            molesOutside = value;
            if(molesOutside == 0)
            {
                Invoke(nameof(ChooseRandomMoles), delayBetweenMoles);
            }
        }
    }

}
