using UnityEngine;
using UnityEngine.SceneManagement;
// Add this if GameEvents.cs is in a different namespace
// using YourProjectNamespace.Events;

public class PausedMenu : MonoBehaviour
{
    public static bool Paused = false;
    public GameObject PauseMenuCanvas;

    void Start()
    {
        Time.timeScale = 1f;
        EventBus.Instance.Subscribe<PauseGameEvent>(ShowPauseMenu);
        EventBus.Instance.Subscribe<ResumeGameEvent>(HidePauseMenu);
    }

    void OnDestroy()
    {
        EventBus.Instance.Unsubscribe<PauseGameEvent>(ShowPauseMenu);
        EventBus.Instance.Unsubscribe<ResumeGameEvent>(HidePauseMenu);
    }

    public void ShowPauseMenu(object eventData)
    {
        Paused = true;
        PauseMenuCanvas.SetActive(true);
        GameManager.Instance.IsPaused = true;
        Time.timeScale = 0f;
    }


    public void HidePauseMenu(object eventData)
    {
        Paused = false;
        PauseMenuCanvas.SetActive(false);
        GameManager.Instance.IsPaused = false;
        Time.timeScale = 1f;
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void HidePauseMenuWrapper()
    {
        HidePauseMenu(null); // Call the existing HidePauseMenu with null
    }

    public void ShowPauseMenuWrapper()
    {
        ShowPauseMenu(null);
    }
}
