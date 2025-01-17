using UnityEngine;

public class GamePauseManager : MonoBehaviour
{
    private bool isPaused = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            DisablePlayerControls();
        }
        else
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            EnablePlayerControls();
        }
    }

    private void DisablePlayerControls()
    {
        var playerController = Object.FindFirstObjectByType<PlayerController>();
        if (playerController != null)
        {
            playerController.enabled = false;
        }
    }
    private void EnablePlayerControls()
    {
        var playerController = Object.FindFirstObjectByType<PlayerController>();
        if (playerController != null)
        {
            playerController.enabled = true;
        }
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}