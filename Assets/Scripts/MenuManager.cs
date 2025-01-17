using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject menuPanel;        // Основная панель меню (MenuPanel)
    public GameObject buttonsPanel;     // Панель с кнопками (Buttons Panel)
    public GameObject settingsPanel;    // Панель настроек (Settings Panel)

    [Header("Menu Buttons")]
    public Button continueButton;       // Кнопка "Continue"
    public Button settingsButton;       // Кнопка "Settings"
    public Button exitButton;           // Кнопка "Exit to Menu"

    private GamePauseManager gamePauseManager;  // Ссылка на GamePauseManager

    void Start()
    {
        // Находим GamePauseManager в сцене
        gamePauseManager = Object.FindFirstObjectByType<GamePauseManager>();

        // Проверяем, был ли найден GamePauseManager
        if (gamePauseManager == null)
        {
            Debug.LogError("GamePauseManager not found in the scene!");
        }

        // Убедимся, что MenuPanel и SettingsPanel скрыты при старте
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        // Добавляем обработчики событий к кнопкам
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(ResumeGame);
        }
        if (settingsButton != null)
        {
            settingsButton.onClick.AddListener(OpenSettings);
        }
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(ExitToMenu);
        }
    }

    void Update()
    {
        // Отслеживаем нажатие на клавишу Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    // Используем GamePauseManager для переключения паузы
    public void TogglePause()
    {
        if (gamePauseManager != null)
        {
            // Переключаем паузу через GamePauseManager
            gamePauseManager.TogglePause();

            // Получаем текущий статус паузы
            bool isPaused = gamePauseManager.IsPaused();

            // Включаем/выключаем MenuPanel и скрываем/показываем кнопки
            if (menuPanel != null)
            {
                menuPanel.SetActive(isPaused);
            }

            if (buttonsPanel != null)
            {
                buttonsPanel.SetActive(isPaused);
            }

            if (settingsPanel != null)
            {
                settingsPanel.SetActive(false); // При открытии меню настройки скрыты
            }
        }
    }

    // Открытие панели настроек
    public void OpenSettings()
    {
        if (buttonsPanel != null)
        {
            buttonsPanel.SetActive(false); // Скрываем Buttons Panel
        }
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true); // Показываем Settings Panel
        }
    }

    // Возобновление игры
    public void ResumeGame()
    {
        if (gamePauseManager != null)
        {
            gamePauseManager.TogglePause(); // Переключаем паузу через GamePauseManager
        }

        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }
    }

    // Выход в главное меню
    public void ExitToMenu()
    {
        if (gamePauseManager != null)
        {
            gamePauseManager.TogglePause(); // Убираем паузу перед выходом
        }

        Debug.Log("Returning to main menu"); // Заглушка для выхода в главное меню
        // Здесь можно загрузить сцену с главным меню, например:
        // SceneManager.LoadScene("MainMenu");
    }
}
