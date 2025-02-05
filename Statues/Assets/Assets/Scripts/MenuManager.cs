using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject bgMusicGameObject;
    [SerializeField] GameObject instructionPanel;
    [SerializeField] TextMeshProUGUI highscore;

    public void PlayGame()
    {
        bgMusicGameObject.GetComponent<AudioSource>().volume = 0.2f;
        SceneManager.LoadScene(2);
    }

    public void OpenInstructions()
    {
        instructionPanel.SetActive(!instructionPanel.activeSelf);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    void Start()
    {
        highscore.text = HighscoreManager.HighScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
