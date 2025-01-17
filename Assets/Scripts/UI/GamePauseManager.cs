using UnityEngine;

public class GamePauseManager : MonoBehaviour
{
    private bool isPaused = false;

    void Start()
    {
        // Скрываем курсор при запуске игры
        Cursor.lockState = CursorLockMode.Locked;  // Блокируем курсор
        Cursor.visible = false;                    // Скрываем курсор
    }

    // Метод для переключения паузы
    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            // Пауза: остановка времени, разблокировка курсора и управление персонажем
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            DisablePlayerControls();
        }
        else
        {
            // Возобновление игры: восстановление времени, блокировка курсора и управление персонажем
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            EnablePlayerControls();
        }
    }

    // Отключение управления персонажем
    private void DisablePlayerControls()
    {
        var playerController = Object.FindFirstObjectByType<PlayerController>();
        if (playerController != null)
        {
            playerController.enabled = false;
        }
    }

    // Включение управления персонажем
    private void EnablePlayerControls()
    {
        var playerController = Object.FindFirstObjectByType<PlayerController>();
        if (playerController != null)
        {
            playerController.enabled = true;
        }
    }

    // Метод для проверки текущего состояния паузы
    public bool IsPaused()
    {
        return isPaused;
    }
}