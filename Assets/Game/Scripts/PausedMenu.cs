using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausedMenu : MonoBehaviour
{
    public static bool Paused = false;
    public GameObject PauseMenuCanvas;

    void Start()
    {
        Time.timeScale = 1f;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    if (GameManager.Instance.IsPaused)
        //    {
        //        EventManager.TriggerResumeGame();
        //    }
        //    else
        //    {
        //        EventManager.TriggerPauseGame();
        //    }
        //}
    }

    private void OnEnable()
    {
        EventManager.OnPauseGame += ShowPauseMenu;
        EventManager.OnResumeGame += HidePauseMenu;
    }

    private void OnDisable()
    {
        EventManager.OnPauseGame -= ShowPauseMenu;
        EventManager.OnResumeGame -= HidePauseMenu;
    }

    public void ShowPauseMenu()
    {
        PauseMenuCanvas.SetActive(true);
        GameManager.Instance.IsPaused = true;
        Time.timeScale = 0f;
    }

    public void HidePauseMenu()
    {
        PauseMenuCanvas.SetActive(false);
        GameManager.Instance.IsPaused = false;
        Time.timeScale = 1f;
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
