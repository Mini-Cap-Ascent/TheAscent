using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour, IGameState
{
    public Canvas pauseMenuUI;

    private void Awake()
    {
        // It's safer to assign this reference in the Unity Editor if possible.
        // If not, move the initialization to Start or Awake for runtime assignment.
    }

    public void UpdateState() { 
    
    }

    public void ResumeState()
    {
    
    }

    private void Start()
    {
        // Ensure the pause menu is hidden by default.
        pauseMenuUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Toggle pause menu with keyboard (e.g., Escape key).
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
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

    // UpdateState and ResumeState could be used for more specific behavior when changing states
    // For simplicity, they're not elaborated here.

    public void TogglePause()
    {
        bool isPaused = pauseMenuUI.gameObject.activeSelf;
        if (!isPaused)
        {
            EnterState(); // Show pause menu and pause the game
            EventManager.TriggerPlayPressed(); // Assuming this means 'Pause' in your context
        }
        else
        {
            ExitState(); // Hide pause menu and resume the game
            EventManager.TriggerExitPressed(); // Assuming this means 'Resume' in your context
        }
    }
}   

