using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region SINGLETON
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    #endregion

    private bool isGameOver = false;

    //Experience gathered in each level
    public int[] experience = new int[3];
    //The experience required for each phase
    public int[] phasesExperience = {0, 300, 1500, 8000 };

    void Awake()
    {
        LoadData();
    }

    public bool IsGameOver { get; set; }

    //Load Game Data
    public void LoadData()
    {
        //Load Experience
        for (int i = 0; i < experience.Length; i++)
        {
            experience[i] = PlayerPrefs.GetInt("ExpLvl_" + i, 0);
        }
    }

    //Save Game Data
    public void SaveData()
    {
        //Save Experience
        for (int i = 0; i < experience.Length; i++)
        {
            PlayerPrefs.SetInt("ExpLvl_" + i, experience[i]);
        }
    }

    //Auto create GameManager
    [RuntimeInitializeOnLoadMethod]
    static void AutoCreate()
    {
        _instance = new GameObject("GameManager").AddComponent<GameManager>();
        DontDestroyOnLoad(_instance.gameObject);
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }
}
