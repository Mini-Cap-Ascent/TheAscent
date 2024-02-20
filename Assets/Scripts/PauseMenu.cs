using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour, IGameState
{
    public GameObject pauseMenuUI;


    public void EnterState()
    {
        pauseMenuUI.SetActive(true);
    }

    public void UpdateState()
    {
        // Optional: Toggle pause menu with keyboard (e.g., Escape key).
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void ExitState()
    {
        pauseMenuUI.SetActive(false);
    }

    public void ResumeState()
    {
        pauseMenuUI.SetActive(false);
    }

    private void Start()
    {
        pauseMenuUI.SetActive(true); // Ensure the pause menu is hidden by default.
    }

    private void Update()
    {
        // Optional: Toggle pause menu with keyboard (e.g., Escape key).
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        bool isPaused = pauseMenuUI.activeSelf;
        pauseMenuUI.SetActive(!isPaused);

        if (!isPaused)
        {
            
            Time.timeScale = 0f; // Pause the game
            EventManager.TriggerPlayPressed(); // Assuming this means 'Pause' in your context
        }
        else
        {
            Time.timeScale = 1f; // Resume the game
            EventManager.TriggerExitPressed(); // Assuming this means 'Resume' in your context
        }
    }
}   

