using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public GameObject DeathMenuCanvas;

    void Start()
    {
        // Initially, the death menu should not be visible.
        DeathMenuCanvas.SetActive(false);

        // Subscribe to a hypothetical death event. You'll need to call TriggerDeathEvent() where you handle the player's death logic.
        EventBus.Instance.Subscribe<DeathEvent>(ShowDeathMenu);
    }

    void OnDestroy()
    {
        // Unsubscribe from the death event to clean up.
        EventBus.Instance.Unsubscribe<DeathEvent>(ShowDeathMenu);
    }

    public void ShowDeathMenu(object eventData)
    {
        DeathMenuCanvas.SetActive(true);
        // Optionally pause game mechanics that should stop upon death.
        Time.timeScale = 0f;
    }

    public void HideDeathMenu()
    {
        DeathMenuCanvas.SetActive(false);
        // Resume gameplay mechanics if needed.
        Time.timeScale = 1f;
    }

    public void RestartLevelButton()
    {
        // Assuming the current scene is the level that needs restarting.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f; // Ensure game time is resumed.
    }

    public void MainMenuButton()
    {
        // Load the main menu scene. Make sure to replace "MainMenu" with the actual name of your main menu scene.
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f; // Ensure game time is resumed.
    }

    public void QuitGameButton()
    {
        // Quit the application.
        Application.Quit();
    }

}
