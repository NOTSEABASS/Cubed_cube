using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject winPanel;
    private void Awake()
    {
        if(Instance == null) Instance = this;
        
    }

    public void WinGame()
    {
        winPanel.SetActive(true);
    }

    public void Relaod()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadLevelsScene()
    {
        SceneManager.LoadScene("Levels");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }
}
