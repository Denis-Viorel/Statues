using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject bgMusicGameObject;
    [SerializeField] GameObject instructionPanel;

    public void PlayGame()
    {
        bgMusicGameObject.GetComponent<AudioSource>().volume = 0.2f;
        SceneManager.LoadScene(1);
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
