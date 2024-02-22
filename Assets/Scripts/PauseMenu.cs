using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour, IGameState
{
    public Canvas pauseMenuUI;
    public Button pauseButton;

    private void Awake()
    {
        pauseButton.onClick.AddListener(TogglePause);
         // Make sure this line is not commented out.
    }

    public void UpdateState() { }

    public void ResumeState() { }

    private void Start()
    {
        pauseMenuUI.gameObject.SetActive(false);
       
    }

    public void EnterState()
    {
        pauseMenuUI.gameObject.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }

    public void ExitState()
    {
        pauseMenuUI.gameObject.SetActive(false);
        Time.timeScale = 1f; // Resume game time
    }

    public void TogglePause()
    {
        bool isPaused = pauseMenuUI.gameObject.activeSelf;
        if (!isPaused)
        {
            EnterState(); // Show pause menu and pause the game
            // Assuming this means 'Pause' in your context
            EventManager.TriggerShowOptionsMenu();
        }
        else
        {
            ExitState(); // Hide pause menu and resume the game
            // Assuming this means 'Resume' in your context
            EventManager.TriggerExitPressed();
        }
    }
}

