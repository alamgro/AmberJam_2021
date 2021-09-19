using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MinigameItem : MonoBehaviour
{
    [Header("Level Info")]
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string levelName;
    [SerializeField] private string levelDescription;
    [SerializeField] private int index; //Must be the same as in the GameManager 
    [SerializeField] private int phaseLevel; //each level has 3 phase
    [SerializeField] private Sprite sprMapping;

    //public Sprite levelSprite;
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI txtLevelName;
    [SerializeField] private TextMeshProUGUI txtDescription;
    [SerializeField] private TextMeshProUGUI txtExperience;
    [SerializeField] private TextMeshProUGUI txtLevelNum;
    [SerializeField] private Image imgMapping;
    [SerializeField] private Image imgExperienceBar;
    //public Image imgLevel;

    void Start()
    {
        phaseLevel = PlayerPrefs.GetInt("PhaseLevel_" + index, 0); //Load the phase number of this minigame
        CheckPhaseLevel(index);

        //UI Setup
        txtLevelName.text = levelName;
        txtDescription.text = levelDescription;
        txtLevelNum.text = "Level " + phaseLevel;
        imgMapping.sprite = sprMapping;
        txtExperience.text = GameManager.Instance.experience[index].ToString() + " / " + GameManager.Instance.phasesExperience[phaseLevel];
        imgExperienceBar.fillAmount = (float)GameManager.Instance.experience[index] / GameManager.Instance.phasesExperience[phaseLevel];
        //imgLevel.sprite = levelSprite;
    }

    public void StartLevel()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void CheckPhaseLevel(int _levelIndex)
    {
        if(GameManager.Instance.experience[index] >= GameManager.Instance.phasesExperience[phaseLevel])
        {
            GameManager.Instance.experience[index] = 0; //Reset experience
            phaseLevel++;
            PlayerPrefs.SetInt("PhaseLevel_" + index, phaseLevel);
        }
    }
}
