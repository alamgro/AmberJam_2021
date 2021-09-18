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
    [SerializeField] private int levelNumber; //Must be the same as in the GameManager 
    //public Sprite levelSprite;
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI txtLevelName;
    [SerializeField] private TextMeshProUGUI txtDescription;
    [SerializeField] private TextMeshProUGUI txtExperience;
    //public Image imgLevel;

    void Start()
    {
        txtLevelName.text = levelName;
        txtDescription.text = levelDescription;
        txtExperience.text = "Experienced gained: " + GameManager.Instance.experience[levelNumber].ToString();
        //imgLevel.sprite = levelSprite;
    }

    public void StartLevel()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
